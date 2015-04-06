using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;

using FTD2XX_NET;

namespace EggPainter
{
    public partial class mainwin : Form
    {
        FTDI myFtdiDevice = null;
        int LEDSTATE = 0;
        int ENNSTATE = 1;
        int TASTE = 0;

        List<cmditem> cmds;
        bool running = false;

        int curr_rot = 0;
        int curr_x = 0;
        int curr_down = 0;
        int step;
        DateTime nextstep;


        public mainwin()
        {
            InitializeComponent();
        }

        public void dolog(string s)
        {
            logbox.AppendText(s);
            logbox.AppendText("\n");
        }

        private void search_ftdi_Click(object sender, EventArgs e)
        {
            UInt32 ftdiDeviceCount = 0;
            FTDI.FT_STATUS ftStatus = FTDI.FT_STATUS.FT_OK;
            myFtdiDevice = new FTDI();

            // Determine the number of FTDI devices connected to the machine
            ftStatus = myFtdiDevice.GetNumberOfDevices(ref ftdiDeviceCount);
            // Check status
            if (ftStatus == FTDI.FT_STATUS.FT_OK)
            {
                dolog("Number of FTDI devices: " + ftdiDeviceCount.ToString());
            }
            else
            {
                // Wait for a key press
                dolog("Failed to get number of devices (error " + ftStatus.ToString() + ")");
                return;
            }

            // If no devices available, return
            if (ftdiDeviceCount == 0)
            {
                // Wait for a key press
                dolog("Failed to get number of devices (error " + ftStatus.ToString() + ")");
                return;
            }

            // Allocate storage for device info list
            FTDI.FT_DEVICE_INFO_NODE[] ftdiDeviceList = new FTDI.FT_DEVICE_INFO_NODE[ftdiDeviceCount];

            // Populate our device list
            ftStatus = myFtdiDevice.GetDeviceList(ftdiDeviceList);

            if (ftStatus == FTDI.FT_STATUS.FT_OK)
            {
                for (UInt32 i = 0; i < ftdiDeviceCount; i++)
                {
                    dolog("Device Index: " + i.ToString());
                    dolog("Flags: " + String.Format("{0:x}", ftdiDeviceList[i].Flags));
                    dolog("Type: " + ftdiDeviceList[i].Type.ToString());
                    dolog("ID: " + String.Format("{0:x}", ftdiDeviceList[i].ID));
                    dolog("Location ID: " + String.Format("{0:x}", ftdiDeviceList[i].LocId));
                    dolog("Serial Number: " + ftdiDeviceList[i].SerialNumber.ToString());
                    dolog("Description: " + ftdiDeviceList[i].Description.ToString());
                }
            }

            ftStatus = myFtdiDevice.OpenBySerialNumber("FTSIGRX2");
            if (ftStatus != FTDI.FT_STATUS.FT_OK)
            {
                // Wait for a key press
                dolog("Failed to open device (error " + ftStatus.ToString() + ")");
                return;
            }

            // myFtdiDevice.SetTimeouts(10, 10);
            ftStatus = myFtdiDevice.SetBitMode(0x75, FTD2XX_NET.FTDI.FT_BIT_MODES.FT_BIT_MODE_ASYNC_BITBANG);
            // ftStatus = myFtdiDevice.SetBitMode(0x00, FTD2XX_NET.FTDI.FT_BIT_MODES.FT_BIT_MODE_ASYNC_BITBANG);
            if (ftStatus != FTDI.FT_STATUS.FT_OK)
            {
                // Wait for a key press
                dolog("Failed to open device (error " + ftStatus.ToString() + ")");
                return;
            }
            myFtdiDevice.Purge(FTD2XX_NET.FTDI.FT_PURGE.FT_PURGE_RX | FTD2XX_NET.FTDI.FT_PURGE.FT_PURGE_TX);
            myFtdiDevice.SetBaudRate(115200);

            dolog("USB-Device now opened");
            itime.Enabled = true;
            setENN(0);
            TMC428_init();
        }

        private void bla_Click(object sender, EventArgs e)
        {
            if (itime.Enabled)
                itime.Enabled = false;
            else
                itime.Enabled = true;
        }

        private void itime_Tick(object sender, EventArgs e)
        {
            byte dummy = 0;
            uint n=0;
            int pos=0;
            int status=0;
            FTDI.FT_STATUS ftStatus = FTDI.FT_STATUS.FT_OK;
            ftStatus = myFtdiDevice.GetPinStates(ref dummy);
            if (ftStatus != FTDI.FT_STATUS.FT_OK)
            {
                // Wait for a key press
                dolog("Failed to read from device (error " + ftStatus.ToString() + ")");
                return;
            }

            if (running)
            {
                DateTime nn = DateTime.Now;
                if (nn > nextstep)
                {
                    step++;
                    if (step >= cmds.Count)
                    {
                        running = false;
                        btn_run.BackColor = Color.Lime;
                        progress.Value = 0;
                        gopos_auto(0, 0, 0);
                        return;
                    }
                    else
                    {
                        cmditem c = cmds[step];
                        simulate_step(step);
                        run_command(c);
                        double rel = (double)step * 100.0 / (double)cmds.Count;
                        progress.Value = (int) Math.Floor(rel);
                    }
                }
            }

            // TMC428_readreg(16 * 2 + 1, ref status, ref pos);
        }


        private byte bencode(int CS, int CLK, int SDO)
        {
            return (byte) (
                ((LEDSTATE != 0) ? 0x40 : 0) |
                ((ENNSTATE != 0) ? 0x20 : 0) |
                ((CLK != 0) ? 0x10 : 0) |
                ((CS != 0) ? 0x04 : 0) |
                ((SDO != 0) ? 0x01 : 0));
        }

        private void setLED(int onoff)
        {
            FTDI.FT_STATUS ftStatus = FTDI.FT_STATUS.FT_OK;
            byte[] dummy = new byte[1];
            uint n=0;
            LEDSTATE = (onoff != 0) ? 1 : 0;
            dummy[0] = bencode(1, 1, 0);
            ftStatus = myFtdiDevice.Write(dummy, 1, ref n);
            if (ftStatus != FTDI.FT_STATUS.FT_OK)
            {
                // Wait for a key press
                dolog("Failed to write to device (error " + ftStatus.ToString() + ")");
                return;
            }
        }

        private void setENN(int onoff)
        {
            FTDI.FT_STATUS ftStatus = FTDI.FT_STATUS.FT_OK;
            byte[] dummy = new byte[1];
            uint n = 0;
            ENNSTATE = (onoff != 0) ? 1 : 0;
            dummy[0] = bencode(1, 1, 0);
            ftStatus = myFtdiDevice.Write(dummy, 1, ref n);
            if (ftStatus != FTDI.FT_STATUS.FT_OK)
            {
                // Wait for a key press
                dolog("Failed to write to device (error " + ftStatus.ToString() + ")");
                return;
            }
        }

        private UInt32 spi_read_write(UInt32 wr)
        {
            FTDI.FT_STATUS ftStatus = FTDI.FT_STATUS.FT_OK;
            byte[] data = new byte[128];
            byte[] indata = new byte[128];
            uint l = 0;
            uint n = 0;
            UInt32 u = 0;
            UInt32 bit = 0x80000000;

            data[l] = bencode(0, 1, 0);l++;
            for (int i=0;i<32;i++) {
                data[l] = bencode(0, 0, (((wr >> (31-i)) & 0x00000001) != 0) ? 1 : 0); l++;
                data[l] = bencode(0, 1, (((wr >> (31-i)) & 0x00000001) != 0) ? 1 : 0); l++;
            }
            data[l] = bencode(1, 1, 0); l++;

            // myFtdiDevice.Purge(FTD2XX_NET.FTDI.FT_PURGE.FT_PURGE_RX | FTD2XX_NET.FTDI.FT_PURGE.FT_PURGE_TX);

            for (int i = 0; i < l; i++)
            {
                byte[] dummy = new byte[1];
                byte din = 0;
                uint txlen = 0;
                dummy[0] = data[i];
                ftStatus = myFtdiDevice.Write(dummy, 1, ref n);
                if (ftStatus != FTDI.FT_STATUS.FT_OK)
                {
                    // Wait for a key press
                    dolog("Failed to write to device (error " + ftStatus.ToString() + ")");
                    return 0;
                }
                do
                {
                    myFtdiDevice.GetTxBytesWaiting(ref txlen);
                } while (txlen > 0);
                ftStatus = myFtdiDevice.GetPinStates(ref din);
                if (ftStatus != FTDI.FT_STATUS.FT_OK)
                {
                    // Wait for a key press
                    dolog("Failed to read from device (error " + ftStatus.ToString() + ")");
                    return 0;
                }
                indata[i] = din;
            }
            /*if (n != l)
            {
                dolog("Failed to read from device");
                return 0;
            }*/
            for (int i = 0; i < 32; i++)
            {
                if ((indata[2 + 2*i] & 0x08) != 0)
                    u |= bit;
                bit = bit / 2;
            }

            return u;
        }

        private UInt32 spi_write(UInt32 wr)
        {
            FTDI.FT_STATUS ftStatus = FTDI.FT_STATUS.FT_OK;
            byte[] data = new byte[128];
            uint l = 0;
            uint n = 0;
            UInt32 u = 0;
            UInt32 bit = 0x80000000;
            uint txlen=0;

            data[l] = bencode(0, 1, 0); l++;
            for (int i = 0; i < 32; i++)
            {
                data[l] = bencode(0, 0, (((wr >> (31-i)) & 0x00000001) != 0) ? 1 : 0); l++;
                data[l] = bencode(0, 1, (((wr >> (31-i)) & 0x00000001) != 0) ? 1 : 0); l++;
            }
            data[l] = bencode(1, 1, 0); l++;

            // myFtdiDevice.Purge(FTD2XX_NET.FTDI.FT_PURGE.FT_PURGE_RX | FTD2XX_NET.FTDI.FT_PURGE.FT_PURGE_TX);

            ftStatus = myFtdiDevice.Write(data, (int)l, ref n);
            if (ftStatus != FTDI.FT_STATUS.FT_OK)
            {
                // Wait for a key press
                dolog("Failed to write to device (error " + ftStatus.ToString() + ")");
                return 0;
            }

            do
            {
                myFtdiDevice.GetTxBytesWaiting(ref txlen);
            } while (txlen > 0);

            return 0;
        }

        private int TMC428_read(int add)
        {
            UInt32 u, a;
            a=(UInt32) add;
            u = ((UInt32)(add & 0x7F) << 25) | 0x01000000;
            return (int)spi_read_write(u);
        }

        private int TMC428_write(int add, int data)
        {
            UInt32 u, a, d;
            a = (UInt32)add;
            d= ((UInt32)data) & 0x00FFFFFF;
            u = ((UInt32)(add & 0x7F) << 25) | d;
            spi_write(u);
            return 0;
        }

        private void TMC428_readreg(int add, ref int status, ref int data) {
            int u;
            u=TMC428_read(add & 0x3f);
            data= u & 0x00FFFFFF;
            status=(u>>24)&0xFF;
        }

        private void TMC428_writereg(int add, int data) {
            int d;
            d=data & 0x00FFFFFF;
            TMC428_write(add & 0x3F,d);
        }

        private void TMC428_readmem(int add, ref int status, ref int bodd, ref int beven) {
            int u;
            u=TMC428_read((add &0x3F)| 0x40);
            beven=u & 0x0000003F;
            bodd=(u & 0x00003F00)>>8;
            status = (u >> 24) & 0xFF;
        }

        private void TMC428_writemem(int add, int bodd, int beven) {
            int u;
            u=(beven & 0x3F) | ((bodd & 0x3F) << 8);
            TMC428_write((add &0x3F) | 0x40,u);
        }

        private void TMC428_set_a_reg(int channel, int is_agtat, int is_aleat, int is_v0, int a_threshold)
        {
            int u;
            u = 
                ((is_agtat & 0x07) << 20) | 
                ((is_aleat & 0x07) << 16) | 
                ((is_v0 & 0x07) << 12) | 
                (a_threshold & 0x3F);
            TMC428_writereg(channel * 16 + 8, u);
        }

        private void TMC428_set_mul_div(int channel, int pmul, int pdiv)
        {
            int u;
            u = (1 << 15) | ((pmul & 0x7F) << 8) | (pdiv & 0x0F);
            TMC428_writereg(channel * 16 + 9, u);
        }

        private void TMC428_set_ref_conf(int channel, int REF_RnL, int SOFT_STOP, 
            int DISABLE_STOP_L, int DISABLE_STOP_R, int rm)
        {
            int u;
            u =((REF_RnL & 0x01) << 11) |
                ((SOFT_STOP & 0x01) << 10) |
                ((DISABLE_STOP_R & 0x01) << 9) |
                ((DISABLE_STOP_L & 0x01) << 8) |
                (rm & 0x03);
            TMC428_writereg(channel * 16 + 10, u);
        }

        private void TMC428_set_intflags(int channel, int int_mask, int int_flags)
        {
            int u;
            u=((int_mask & 0xFF)<<8) | (int_flags & 0xFF);
            TMC428_writereg(channel*16+11,u);
        }

        private void TMC428_set_divs(int channel, int pulse_div, int ramp_div, int usrs)
        {
            int u;
            u = ((pulse_div & 0x0F) << 12) | ((ramp_div & 0x0F) << 8) | (usrs & 0x07);
            TMC428_writereg(channel * 16 + 12, u);
        }

        private void TMC428_setup(int mot1r, int refmux, int cont_update, int clk2_div, int cs_comlnd,
            int pol_DAC_AB, int pol_FD_AB, int pol_PH_AB, int pol_SCK_S, int pol_nSCS_S, int LSMD)
        {
            int u;
            u = ((mot1r & 0x01) << 21) |
                ((refmux & 0x01) << 20) |
                ((cont_update & 0x01) << 16) |
                ((clk2_div & 0xFF) << 8) |
                ((cs_comlnd & 0x01) << 7) |
                ((pol_DAC_AB & 0x01) << 6) |
                ((pol_FD_AB & 0x01) << 5) |
                ((pol_PH_AB & 0x01) << 4) |
                ((pol_SCK_S & 0x01) << 3) |
                ((pol_nSCS_S & 0x01) << 2) |
                (LSMD & 0x03);
            TMC428_writereg(63, u);
        }

        private void TMC428_setcover(int cover_pos, int cover_len, int cover)
        {
            int u;
            u = ((cover_pos & 0x3F) << 8) | (cover_len & 0x1F);
            TMC428_writereg(26, u);
            TMC428_writereg(27, cover);
        }

        private int TMC428_readrefswitches()
        {
            int stat=0;
            int data=0;
            TMC428_readreg(62, ref stat, ref data);
            return (data & 0x3F);
        }

        private void TMC428_init() {
/*            TMC428_writemem(0x00, 0x05, 0x10);
            TMC428_writemem(0x01, 0x03, 0x04);
            TMC428_writemem(0x02, 0x06, 0x02);
            TMC428_writemem(0x03, 0x0D, 0x10);
            TMC428_writemem(0x04, 0x0B, 0x0C);
            TMC428_writemem(0x05, 0x2E, 0x0A);

            TMC428_writemem(0x06, 0x05, 0x10);
            TMC428_writemem(0x07, 0x03, 0x04);
            TMC428_writemem(0x08, 0x06, 0x02);
            TMC428_writemem(0x09, 0x0D, 0x10);
            TMC428_writemem(0x0A, 0x0B, 0x0C);
            TMC428_writemem(0x0B, 0x2E, 0x0A);

            TMC428_writemem(0x0C, 0x05, 0x10);
            TMC428_writemem(0x0D, 0x03, 0x04);
            TMC428_writemem(0x0E, 0x06, 0x02);
            TMC428_writemem(0x0F, 0x0D, 0x10);
            TMC428_writemem(0x10, 0x0B, 0x0C);
            TMC428_writemem(0x11, 0x2E, 0x0A);*/

            setENN(1);
            
            TMC428_writemem(0x00, 0x05, 0x10);
            TMC428_writemem(0x01, 0x03, 0x04);
            TMC428_writemem(0x02, 0x06, 0x02);
            TMC428_writemem(0x03, 0x0D, 0x10);
            TMC428_writemem(0x04, 0x0B, 0x0C);
            TMC428_writemem(0x05, 0x2E, 0x0A);
            
            TMC428_writemem(0x06, 0x05, 0x10);
            TMC428_writemem(0x07, 0x03, 0x04);
            TMC428_writemem(0x08, 0x06, 0x02);
            TMC428_writemem(0x09, 0x0D, 0x10);
            TMC428_writemem(0x0A, 0x0B, 0x0C);
            TMC428_writemem(0x0B, 0x2E, 0x0A);

            // TMC428_writemem(0x0C, 0x05, 0x07);
            TMC428_writemem(0x0C, 0x05, 0x10);
            TMC428_writemem(0x0D, 0x03, 0x04);
            TMC428_writemem(0x0E, 0x06, 0x02);
            TMC428_writemem(0x0F, 0x0D, 0x0F);
            TMC428_writemem(0x10, 0x0B, 0x0C);
            TMC428_writemem(0x11, 0x2E, 0x0A);

            for (int i = 0; i < 32; i++)
            {
                int y1 = (int)Math.Round(64 * Math.Sin(0.5 * 3.141592 * (2 * i + 0) / 64));
                int y2 = (int)Math.Round(64 * Math.Sin(0.5 * 3.141592 * (2 * i + 1) / 64));
                if (y1 < 0) y1 = 0;
                if (y1 > 63) y1 = 63;
                if (y2 < 0) y2 = 0;
                if (y2 > 63) y2 = 63;
                TMC428_writemem(32+i, y2, y1);
            }

            trackX.Value = 0;
            trackY.Value = 0;
            trackZ.Value = 0;

            /*
            TMC428_writemem(0x00,0x05,0x10);
            TMC428_writemem(0x02,0x03,0x04);
            TMC428_writemem(0x04,0x06,0x02);
            TMC428_writemem(0x06,0x0D,0x10);
            TMC428_writemem(0x08,0x0B,0x0C);
            TMC428_writemem(0x0a,0x2E,0x0A);
            TMC428_writemem(0x0c,0x05,0x10);
            TMC428_writemem(0x0e,0x03,0x04);
            TMC428_writemem(0x10,0x06,0x02);
            TMC428_writemem(0x12,0x0D,0x10);
            TMC428_writemem(0x14,0x0B,0x0C);
            TMC428_writemem(0x16,0x2E,0x0A);
            TMC428_writemem(0x18,0x05,0x07);
            TMC428_writemem(0x1a,0x03,0x04);
            TMC428_writemem(0x1c,0x06,0x02);
            TMC428_writemem(0x1e,0x0D,0x0F);
            TMC428_writemem(0x20,0x0B,0x0C);
            TMC428_writemem(0x22,0x2E,0x0A);
            */
            /*
            for (int i = 0; i < 64;i++ )
            {
                TMC428_writemem(i, 0x00, 0x00);
            }
            */
            TMC428_setup(
                0, // mot1r is ignopred when refux=1
                1, // refmux
                1, // cont. Update
                7, // clk2-divider --> clk2 = 500kHz
                0, // common cs (0 -> mit TMC246)
                0, // pol_DAC 
                0, // pol_FD ??
                0, // pol_PhaseAB
                0, 0, // pol_sck und pol_cs
                2); // LSMD
            TMC428_setcover(0, 0, 0);
            for (int i = 0; i < 3; i++)
            {
                TMC428_writereg(i * 16 + 0, 0x000000); // target pos
                TMC428_writereg(i * 16 + 1, 0x000000); // actual pos
                // TMC428_writereg(i * 16 + 2, 1500); // v_min
                // TMC428_writereg(i * 16 + 2, 100); // v_min
                TMC428_writereg(i * 16 + 2, 100); // v_min
                //TMC428_writereg(i * 16 + 3, 2000); // v_max
                TMC428_writereg(i * 16 + 3, 1000); // v_max
                TMC428_writereg(i * 16 + 4, 0x000000); // v_target
                TMC428_writereg(i * 16 + 5, 0x000000); // actual v
                TMC428_writereg(i * 16 + 6, 2040); // a_max
                //TMC428_writereg(i * 16 + 6, 1000); // a_max
                TMC428_writereg(i * 16 + 7, 0); // actual a
                TMC428_set_a_reg(i, 7, 2, 2, 300);
                TMC428_set_mul_div(i, 114, 1);
                TMC428_set_ref_conf(i, 0, 1, 1, 1,0); // RAMP MODE
                TMC428_set_intflags(i, 0, 0);
                TMC428_set_divs(i, 11, 11, 0); // Pulse_div, ramp_div, microsteps
                TMC428_writereg(i * 16 + 13, 0);        // dx_ref_tolerance
                // TMC428_writereg(i * 16 + 14, 0);
            }
            setENN(0);
        }

        private void butt0_Click(object sender, EventArgs e)
        {
            int i;
            int sta = -1;
            dolog("Register Sets");
            for (i=0;i<63;i++) {
                int u = 0;
                int stain = 0;
                TMC428_readreg(i, ref stain, ref u);
                if (stain != sta) {
                    sta=stain;
                    dolog(String.Format("STATUS=0x{0:X}",sta));
                }
                dolog(
                    String.Format("0x{0:X}=0x{1:X}",i,u)
                    );
            }
            dolog("Memory Sets");
            for (i = 0; i < 63; i++)
            {
                int u = 0;
                int stain = 0;
                int b1 = 0, b2 = 0;
                TMC428_readmem(i, ref stain, ref b1, ref b2);
                if (stain != sta) {
                    sta=stain;
                    dolog(String.Format("STATUS=0x{0:X}",sta));
                }
                dolog(
                    String.Format("0x{0:X}=0x{1:X}, 0x{2:X}", i, b1,b2)
                    );
            }
        }


        private void red0_Click(object sender, EventArgs e)
        {
            gopos_auto(0, 0, 0);
        }


        private void quit_Click(object sender, EventArgs e)
        {
            if (myFtdiDevice != null)
            {
                setENN(1);
            }
            this.Close();
        }

        private void ENNBOX_CheckedChanged(object sender, EventArgs e)
        {
            if (ENNBOX.Checked)
                setENN(0);
            else
                setENN(1);
        }

        private void gopos_auto(int rot, int x, int down)
        {
            if (down != curr_down) TMC428_writereg(0, down * 200);
            if (x != curr_x) TMC428_writereg(16, x);
            if (rot != curr_rot) TMC428_writereg(32, rot * 1000 / 3600);
            curr_rot = rot;
            curr_x = x;
            curr_down = down;
            trackX.Value = rot;
            trackY.Value = x;
            trackZ.Value = down;
        }

        private void gopos_manual(int rot, int x, int down)
        {
            if (down != curr_down) TMC428_writereg(0, down*200);
            if (x != curr_x) TMC428_writereg(16, x);
            if (rot != curr_rot) TMC428_writereg(32, rot*1000/3600);
            curr_rot = rot;
            curr_x = x;
            curr_down = down;
        }

        private void liftpen()
        {
            gopos_auto(curr_rot, curr_x, 0);
        }

        private void trackX_Scroll(object sender, EventArgs e)
        {
            gopos_manual(trackX.Value, trackY.Value, trackZ.Value);
        }

        private void trackY_Scroll(object sender, EventArgs e)
        {
            gopos_manual(trackX.Value, trackY.Value, trackZ.Value);
        }

        private void trackZ_Scroll(object sender, EventArgs e)
        {
            gopos_manual(trackX.Value, trackY.Value, trackZ.Value);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            TMC428_init();
        }

        private void transform(int x, int rot, int wid, int heig, out float rx, out float ry)
        {
            float w = (float)wid / 800.0f;
            float wo = wid / 2;
            float r = (float)heig / 3600.0f;
            float ro = heig / 2;

            rx = wo - (float)x * w;
            ry = ro - (float)rot * r;
        }

        private void simulate()
        {
            // Simulate
            Graphics g = pbox.CreateGraphics();
            g.Clear(Color.White);
            int down = 0;
            int lastrot = 0;
            int lastx = 0;
            Pen p = new Pen(Color.Black);
            Pen q = new Pen(Color.Red);
            //float w = (float)pbox.Width / 800.0f;
            //float wo = pbox.Width / 2;
            //float r = (float)pbox.Height / 3600.0f;
            //float ro = pbox.Height / 2;
            for (int i = 0; i < cmds.Count; i++)
            {
                float ax = 0;
                float ay = 0;
                float bx = 0;
                float by = 0;
                transform(cmds[i].x, cmds[i].rot,pbox.Width, pbox.Height,out ax, out ay);

                g.DrawEllipse(q, ax - 1f, ay - 1f, 2f, 2f);
                if (down == 1)
                {
                    transform(lastx, lastrot, pbox.Width, pbox.Height, out bx, out by);
                    g.DrawLine(p, bx, by, ax, ay);
                }
                lastx = cmds[i].x;
                lastrot = cmds[i].rot;
                down = cmds[i].down;
            }
        }

        int os_down=0;
        int os_lastrot = 0;
        int os_lastx = 0;
        Graphics os_g = null;
        Pen os_p = null;

        private void simulate_step(int i)
        {
            if (i == 0)
            {
                if (os_g == null) 
                    os_g = operate_box.CreateGraphics();
                os_g.Clear(Color.White);
                if (os_p == null)
                    os_p=new Pen(Color.Black);
                os_down = 0;
                os_lastrot = 0;
                os_lastx = 0;
            }
            float ax = 0;
            float ay = 0;
            float bx = 0;
            float by = 0;

            transform(cmds[i].x, cmds[i].rot,operate_box.Width, operate_box.Height,out ax, out ay);
            if (os_down == 1)
            {
                transform(os_lastx, os_lastrot, operate_box.Width, operate_box.Height, out bx, out by);
                os_g.DrawLine(os_p, bx, by, ax, ay);
            }
            os_lastx = cmds[i].x;
            os_lastrot = cmds[i].rot;
            os_down = cmds[i].down;
        }

        private void loadfile(String fn) {
            string line;
            // cmds = new List<cmditem>();
            System.IO.StreamReader f = new System.IO.StreamReader(fn);
            while ((line = f.ReadLine()) != null)
            {
                cmditem c = new cmditem(line);
                if (c.valid != 0)
                {
                    cmds.Add(c);
                }
            }
            f.Close();

            simulate();


            // pbox.Refresh();
        }

        private void savefile(String fn)
        {
            System.IO.StreamWriter f = new System.IO.StreamWriter(fn);
            for (int i = 0; i < cmds.Count; i++)
            {
                string e = cmds[i].codeline();
                f.WriteLine(e);
            }
            f.Close();
        }

        private void btn_load_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter="Egg Files|*.egg";
            DialogResult r = ofd.ShowDialog();
            if (r == DialogResult.OK)
            {
                String fn = ofd.FileName;
                loadfile(fn);
            }
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Egg Files|*.egg";
            DialogResult r = sfd.ShowDialog();
            if (r == DialogResult.OK)
            {
                String fn = sfd.FileName;
                savefile(fn);
            }

        }

        static int abs(int m) { if (m < 0) return -m; else return m; }

        void run_command(cmditem c)
        {
            int mx = abs(c.x - curr_x);
            int mrot = abs((c.rot - curr_rot) * 1000 / 3600);
            int mdown = abs((c.down - curr_down) * 400);
            if (mrot > mx) mx = mrot;
            if (mdown > mx) mx = mdown;

            DateTime dt = DateTime.Now;
            // nextstep = dt.AddMilliseconds(mx * 20);
            nextstep = dt.AddMilliseconds(mx * 0);
            gopos_auto(c.rot, c.x, c.down);
        }

        private void btn_run_Click(object sender, EventArgs e)
        {
            if (!running)
            {
                if (cmds.Count > 0)
                {
                    running = true;
                    btn_run.BackColor = Color.Red;
                    step = 0;
                    progress.Value = 0;
                    simulate_step(0);
                    run_command(cmds[step]);
                }
            }
            else
            {
                running = false;
                btn_run.BackColor = Color.Lime;
                progress.Value = 0;
                liftpen();
            }
        }


        void draw_sector(int cx, int cy, int rad, float phi1, float phi2, int pnts)
        {
            int i;
            float aspect = 1.5f;
            float ax, ay;
            for (i = 0; i < pnts; i++)
            {
                float phi = phi1 + (float)i / pnts * (phi2-phi1);
                ax = (float)rad * (float)Math.Cos(phi);
                ay = (float)rad * (float)Math.Sin(phi);
                ay /= aspect;

                if (i == 0)
                {
                    cmds.Add(new cmditem((int)Math.Floor(cx + ax + 0.5f), (int)Math.Floor(cy + ay + 0.5f), 0));
                }
                cmds.Add(new cmditem((int)Math.Floor(cx + ax + 0.5f), (int)Math.Floor(cy + ay + 0.5f), 1));
            }
            ax = (float)rad * (float)Math.Cos(phi1);
            ay = (float)rad * (float)Math.Sin(phi1);

            ay /= aspect;

            cmds.Add(new cmditem((int)Math.Floor(cx + ax + 0.5f), (int)Math.Floor(cy + ay + 0.5f), 0));
            
        }

        void draw_cloud(int cx, int cy, int w, int h)
        {
            int i;
            float aspect = 1.5f;
            float ax, ay;
            float bx, by;


        }

        void draw_circle(int cx, int cy, int rad, int pnts,float asp, float phio)
        {
            int i;
            float aspect = 1.5f;
            float ax, ay;
            float bx, by;
            for (i = 0; i < pnts; i++)
            {
                float phi = (float)i / pnts * 2.0f * (float)Math.PI;
                ax = (float)rad * (float)Math.Cos(phi)*asp;
                ay = (float)rad * (float)Math.Sin(phi)/asp;

                bx = ax * (float)Math.Cos(phio) - ay * (float)Math.Sin(phio);
                by = ay * (float)Math.Cos(phio) + ax * (float)Math.Sin(phio);

                ax = bx;
                ay = by;
               
                ay/=aspect;
                if (i == 0)
                {
                    cmds.Add(new cmditem((int)Math.Floor(cx+ax + 0.5f), (int)Math.Floor(cy+ay + 0.5f), 0));
                }
                cmds.Add(new cmditem((int)Math.Floor(cx+ax + 0.5f), (int)Math.Floor(cy+ay + 0.5f), 1));
            }
            ax = (float)rad * (float)Math.Cos(0)*asp;
            ay = (float)rad * (float)Math.Sin(0) / aspect/asp;

            bx = ax * (float)Math.Cos(phio) - ay * (float)Math.Sin(phio);
            by = ay * (float)Math.Cos(phio) + ax * (float)Math.Sin(phio);

            ax = bx;
            ay = by;

            ay /= aspect;
            
            
            cmds.Add(new cmditem((int) Math.Floor(cx+ax+0.5f), (int) Math.Floor(cy+ay+0.5f), 0));
        }

        private void flw_add_Click(object sender, EventArgs e)
        {
            int oy = -Decimal.ToInt32(flw_x.Value);
            int ox = Decimal.ToInt32(flw_y.Value);
            float h = Decimal.ToInt32(flw_h.Value);
            float phio = Decimal.ToInt32(flw_phi.Value);
            float aspect = 1.5f;

            if (cmds == null)
                cmds = new List<cmditem>();

            for (int i = 0; i < 8; i++)
            {
                float phi = (float)i / 8 * 2.0f * (float)Math.PI + phio /180f * (float)Math.PI;
                float ax = (float)2*h * (float)Math.Cos(phi);
                float ay = (float)2*h * (float)Math.Sin(phi)/aspect;
                draw_circle((int)Math.Floor(ox + ax+0.5),(int)Math.Floor(oy + ay+0.5), (int)h, 20,1.5f,phi);
            }
            draw_circle(ox, oy, (int)h, 20,1,0);

            simulate();
        }

        private void cmd_clear_Click(object sender, EventArgs e)
        {
            if (running) return;
            cmds = new List<cmditem>();
            simulate();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private float wrap(float i, float lm)
        {
            while (i > lm)
                i -= 2*lm;
            while (i < -lm)
                i += 2 * lm;
            return i;
        }

        private void lj_draw_Click(object sender, EventArgs e)
        {
            int ox = -Decimal.ToInt32(lj_x.Value);
            int orot = Decimal.ToInt32(lj_y.Value);
            float w = Decimal.ToInt32(lj_w.Value);
            float h = Decimal.ToInt32(lj_h.Value);
            float f1 = Decimal.ToInt32(lj_f1.Value);
            float f2 = Decimal.ToInt32(lj_f2.Value);
            float op1 = Decimal.ToInt32(lj_phi1.Value);
            float op2 = Decimal.ToInt32(lj_phi2.Value);
            bool rotate = lj_rotate.Checked;

            if (cmds == null)
                cmds = new List<cmditem>();
 

            float t;
            float dt = 1;
            bool first=true;
            float x = 0;
            float r = 0;

            op1 *= (float)Math.PI / 180.0f;
            op2 *= (float)Math.PI / 180.0f;

            if (rotate)
            {
                for (t = 0; t <= 360.0f; t += dt)
                {
                    float phi = t / 180.0f * (float)Math.PI;
                    x = ox + w * (float)Math.Sin(phi * f1+op1);
                    float nr = wrap(orot + 1800f * (f2 * phi +op2)/ (float)Math.PI, 1800f);
                    if (first)
                    {
                        r = nr;
                        cmds.Add(new cmditem(r, x, 0));
                        first = false;
                    }
                    else
                    {
                        if (Math.Abs(nr - r) > 1800)
                        {
                            cmds.Add(new cmditem(r, x, 0));
                            cmds.Add(new cmditem(nr, x, 0));
                        }
                        r = nr;
                        cmds.Add(new cmditem(r, x, 1));
                    }
                }
                cmds.Add(new cmditem(r, x, 0));
            }
            else
            {

                for (t = 0; t <= 360.0f; t += dt)
                {
                    float phi = t / 180.0f * (float)Math.PI;
                    x = ox + w * (float)Math.Sin(phi * f1 + op1);
                    r = orot + h * (float)Math.Sin(phi * f2 + op2);

                    if (first)
                    {
                        cmds.Add(new cmditem(r, x, 0));
                        first = false;
                    }
                    cmds.Add(new cmditem(r, x, 1));
                }
                cmds.Add(new cmditem(r, x, 0));
            }
            simulate();

        }

        private void mainwin_Load(object sender, EventArgs e)
        {

        }

        private void svg_load_Click(object sender, EventArgs e)
        {
            int ox = -Decimal.ToInt32(svg_x.Value);
            int oy = Decimal.ToInt32(svg_y.Value);
            int svg_size = Decimal.ToInt32(svg_w.Value);

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "SVG Files|*.svg";
            DialogResult r = ofd.ShowDialog();
            if (r != DialogResult.OK)
                return;
            string fn = ofd.FileName;

            svg_read svr = new svg_read(fn);
            svr.aspect = 2.0;
            svr.scaleto = svg_size;
            svr.posx = oy;
            svr.posy = ox;

            List<cmditem> cl = svr.read();
            if (svr.error != null) {
                dolog(svr.error);
            }
            if (cmds == null) 
                cmds = cl;
            else
                cmds.AddRange(cl);
            simulate();

        }

        private void mainwin_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (myFtdiDevice != null)
            {
                setENN(1);
            }
        }



     }
}