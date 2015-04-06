namespace EggPainter
{
    partial class mainwin
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.logbox = new System.Windows.Forms.RichTextBox();
            this.search_ftdi = new System.Windows.Forms.Button();
            this.itime = new System.Windows.Forms.Timer(this.components);
            this.red0 = new System.Windows.Forms.Button();
            this.trackX = new System.Windows.Forms.TrackBar();
            this.trackY = new System.Windows.Forms.TrackBar();
            this.trackZ = new System.Windows.Forms.TrackBar();
            this.ENNBOX = new System.Windows.Forms.CheckBox();
            this.btn_run = new System.Windows.Forms.Button();
            this.progress = new System.Windows.Forms.ProgressBar();
            this.btn_load = new System.Windows.Forms.Button();
            this.pbox = new System.Windows.Forms.PictureBox();
            this.flw_add = new System.Windows.Forms.Button();
            this.flw_y = new System.Windows.Forms.NumericUpDown();
            this.flw_x = new System.Windows.Forms.NumericUpDown();
            this.flw_h = new System.Windows.Forms.NumericUpDown();
            this.flw_phi = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.cmd_clear = new System.Windows.Forms.Button();
            this.lj_draw = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.lj_w = new System.Windows.Forms.NumericUpDown();
            this.lj_y = new System.Windows.Forms.NumericUpDown();
            this.lj_x = new System.Windows.Forms.NumericUpDown();
            this.lj_rotate = new System.Windows.Forms.CheckBox();
            this.lj_h = new System.Windows.Forms.NumericUpDown();
            this.lj_f1 = new System.Windows.Forms.NumericUpDown();
            this.lj_f2 = new System.Windows.Forms.NumericUpDown();
            this.lj_phi1 = new System.Windows.Forms.NumericUpDown();
            this.lj_phi2 = new System.Windows.Forms.NumericUpDown();
            this.btn_save = new System.Windows.Forms.Button();
            this.svg_load = new System.Windows.Forms.Button();
            this.svg_y = new System.Windows.Forms.NumericUpDown();
            this.svg_x = new System.Windows.Forms.NumericUpDown();
            this.svg_w = new System.Windows.Forms.NumericUpDown();
            this.operate_box = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.trackX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackZ)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.flw_y)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.flw_x)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.flw_h)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.flw_phi)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lj_w)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lj_y)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lj_x)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lj_h)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lj_f1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lj_f2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lj_phi1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lj_phi2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.svg_y)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.svg_x)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.svg_w)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.operate_box)).BeginInit();
            this.SuspendLayout();
            // 
            // logbox
            // 
            this.logbox.Location = new System.Drawing.Point(13, 60);
            this.logbox.Margin = new System.Windows.Forms.Padding(4);
            this.logbox.Name = "logbox";
            this.logbox.Size = new System.Drawing.Size(434, 155);
            this.logbox.TabIndex = 0;
            this.logbox.Text = "";
            // 
            // search_ftdi
            // 
            this.search_ftdi.Location = new System.Drawing.Point(13, 13);
            this.search_ftdi.Margin = new System.Windows.Forms.Padding(4);
            this.search_ftdi.Name = "search_ftdi";
            this.search_ftdi.Size = new System.Drawing.Size(437, 39);
            this.search_ftdi.TabIndex = 1;
            this.search_ftdi.Text = "Connect Stepper";
            this.search_ftdi.UseVisualStyleBackColor = true;
            this.search_ftdi.Click += new System.EventHandler(this.search_ftdi_Click);
            // 
            // itime
            // 
            this.itime.Interval = 200;
            this.itime.Tick += new System.EventHandler(this.itime_Tick);
            // 
            // red0
            // 
            this.red0.Location = new System.Drawing.Point(461, 360);
            this.red0.Margin = new System.Windows.Forms.Padding(4);
            this.red0.Name = "red0";
            this.red0.Size = new System.Drawing.Size(155, 37);
            this.red0.TabIndex = 6;
            this.red0.Text = "Move ZERO";
            this.red0.UseVisualStyleBackColor = true;
            this.red0.Click += new System.EventHandler(this.red0_Click);
            // 
            // trackX
            // 
            this.trackX.LargeChange = 16;
            this.trackX.Location = new System.Drawing.Point(461, 30);
            this.trackX.Margin = new System.Windows.Forms.Padding(4);
            this.trackX.Maximum = 1800;
            this.trackX.Minimum = -1800;
            this.trackX.Name = "trackX";
            this.trackX.Size = new System.Drawing.Size(318, 56);
            this.trackX.TabIndex = 14;
            this.trackX.TickFrequency = 100;
            this.trackX.Scroll += new System.EventHandler(this.trackX_Scroll);
            // 
            // trackY
            // 
            this.trackY.LargeChange = 16;
            this.trackY.Location = new System.Drawing.Point(461, 75);
            this.trackY.Margin = new System.Windows.Forms.Padding(4);
            this.trackY.Maximum = 200;
            this.trackY.Minimum = -200;
            this.trackY.Name = "trackY";
            this.trackY.Size = new System.Drawing.Size(318, 56);
            this.trackY.TabIndex = 13;
            this.trackY.TickFrequency = 10;
            this.trackY.Scroll += new System.EventHandler(this.trackY_Scroll);
            // 
            // trackZ
            // 
            this.trackZ.LargeChange = 16;
            this.trackZ.Location = new System.Drawing.Point(461, 120);
            this.trackZ.Margin = new System.Windows.Forms.Padding(4);
            this.trackZ.Maximum = 1;
            this.trackZ.Name = "trackZ";
            this.trackZ.Size = new System.Drawing.Size(318, 56);
            this.trackZ.TabIndex = 14;
            this.trackZ.Scroll += new System.EventHandler(this.trackZ_Scroll);
            // 
            // ENNBOX
            // 
            this.ENNBOX.AutoSize = true;
            this.ENNBOX.Checked = true;
            this.ENNBOX.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ENNBOX.Location = new System.Drawing.Point(461, 170);
            this.ENNBOX.Margin = new System.Windows.Forms.Padding(4);
            this.ENNBOX.Name = "ENNBOX";
            this.ENNBOX.Size = new System.Drawing.Size(177, 21);
            this.ENNBOX.TabIndex = 15;
            this.ENNBOX.Text = "Enable Stepper Drivers";
            this.ENNBOX.UseVisualStyleBackColor = true;
            this.ENNBOX.CheckedChanged += new System.EventHandler(this.ENNBOX_CheckedChanged);
            // 
            // btn_run
            // 
            this.btn_run.BackColor = System.Drawing.Color.Lime;
            this.btn_run.Location = new System.Drawing.Point(623, 360);
            this.btn_run.Name = "btn_run";
            this.btn_run.Size = new System.Drawing.Size(156, 37);
            this.btn_run.TabIndex = 16;
            this.btn_run.Text = "Run";
            this.btn_run.UseVisualStyleBackColor = false;
            this.btn_run.Click += new System.EventHandler(this.btn_run_Click);
            // 
            // progress
            // 
            this.progress.Location = new System.Drawing.Point(461, 337);
            this.progress.Name = "progress";
            this.progress.Size = new System.Drawing.Size(318, 16);
            this.progress.TabIndex = 17;
            // 
            // btn_load
            // 
            this.btn_load.Location = new System.Drawing.Point(461, 269);
            this.btn_load.Name = "btn_load";
            this.btn_load.Size = new System.Drawing.Size(155, 23);
            this.btn_load.TabIndex = 19;
            this.btn_load.Text = "Load";
            this.btn_load.UseVisualStyleBackColor = true;
            this.btn_load.Click += new System.EventHandler(this.btn_load_Click);
            // 
            // pbox
            // 
            this.pbox.BackColor = System.Drawing.Color.White;
            this.pbox.Location = new System.Drawing.Point(461, 403);
            this.pbox.Name = "pbox";
            this.pbox.Size = new System.Drawing.Size(156, 500);
            this.pbox.TabIndex = 20;
            this.pbox.TabStop = false;
            // 
            // flw_add
            // 
            this.flw_add.Location = new System.Drawing.Point(261, 388);
            this.flw_add.Name = "flw_add";
            this.flw_add.Size = new System.Drawing.Size(75, 23);
            this.flw_add.TabIndex = 27;
            this.flw_add.Text = "Draw";
            this.flw_add.UseVisualStyleBackColor = true;
            this.flw_add.Click += new System.EventHandler(this.flw_add_Click);
            // 
            // flw_y
            // 
            this.flw_y.Location = new System.Drawing.Point(13, 417);
            this.flw_y.Maximum = new decimal(new int[] {
            1800,
            0,
            0,
            0});
            this.flw_y.Minimum = new decimal(new int[] {
            1800,
            0,
            0,
            -2147483648});
            this.flw_y.Name = "flw_y";
            this.flw_y.Size = new System.Drawing.Size(120, 22);
            this.flw_y.TabIndex = 29;
            this.flw_y.Value = new decimal(new int[] {
            1200,
            0,
            0,
            0});
            // 
            // flw_x
            // 
            this.flw_x.Location = new System.Drawing.Point(13, 389);
            this.flw_x.Maximum = new decimal(new int[] {
            400,
            0,
            0,
            0});
            this.flw_x.Minimum = new decimal(new int[] {
            400,
            0,
            0,
            -2147483648});
            this.flw_x.Name = "flw_x";
            this.flw_x.Size = new System.Drawing.Size(120, 22);
            this.flw_x.TabIndex = 28;
            // 
            // flw_h
            // 
            this.flw_h.Location = new System.Drawing.Point(139, 388);
            this.flw_h.Maximum = new decimal(new int[] {
            400,
            0,
            0,
            0});
            this.flw_h.Name = "flw_h";
            this.flw_h.Size = new System.Drawing.Size(120, 22);
            this.flw_h.TabIndex = 30;
            this.flw_h.Value = new decimal(new int[] {
            75,
            0,
            0,
            0});
            // 
            // flw_phi
            // 
            this.flw_phi.Location = new System.Drawing.Point(139, 416);
            this.flw_phi.Maximum = new decimal(new int[] {
            180,
            0,
            0,
            0});
            this.flw_phi.Minimum = new decimal(new int[] {
            180,
            0,
            0,
            -2147483648});
            this.flw_phi.Name = "flw_phi";
            this.flw_phi.Size = new System.Drawing.Size(120, 22);
            this.flw_phi.TabIndex = 31;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 369);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 17);
            this.label1.TabIndex = 32;
            this.label1.Text = "Flower";
            // 
            // cmd_clear
            // 
            this.cmd_clear.Location = new System.Drawing.Point(461, 240);
            this.cmd_clear.Name = "cmd_clear";
            this.cmd_clear.Size = new System.Drawing.Size(318, 23);
            this.cmd_clear.TabIndex = 34;
            this.cmd_clear.Text = "Clear List";
            this.cmd_clear.UseVisualStyleBackColor = true;
            this.cmd_clear.Click += new System.EventHandler(this.cmd_clear_Click);
            // 
            // lj_draw
            // 
            this.lj_draw.Location = new System.Drawing.Point(265, 501);
            this.lj_draw.Name = "lj_draw";
            this.lj_draw.Size = new System.Drawing.Size(75, 23);
            this.lj_draw.TabIndex = 35;
            this.lj_draw.Text = "Draw";
            this.lj_draw.UseVisualStyleBackColor = true;
            this.lj_draw.Click += new System.EventHandler(this.lj_draw_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 480);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 17);
            this.label3.TabIndex = 36;
            this.label3.Text = "Lissajous";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // lj_w
            // 
            this.lj_w.Location = new System.Drawing.Point(139, 502);
            this.lj_w.Maximum = new decimal(new int[] {
            400,
            0,
            0,
            0});
            this.lj_w.Name = "lj_w";
            this.lj_w.Size = new System.Drawing.Size(120, 22);
            this.lj_w.TabIndex = 39;
            this.lj_w.Value = new decimal(new int[] {
            200,
            0,
            0,
            0});
            // 
            // lj_y
            // 
            this.lj_y.Location = new System.Drawing.Point(13, 531);
            this.lj_y.Maximum = new decimal(new int[] {
            1800,
            0,
            0,
            0});
            this.lj_y.Minimum = new decimal(new int[] {
            1800,
            0,
            0,
            -2147483648});
            this.lj_y.Name = "lj_y";
            this.lj_y.Size = new System.Drawing.Size(120, 22);
            this.lj_y.TabIndex = 38;
            // 
            // lj_x
            // 
            this.lj_x.Location = new System.Drawing.Point(13, 503);
            this.lj_x.Maximum = new decimal(new int[] {
            400,
            0,
            0,
            0});
            this.lj_x.Minimum = new decimal(new int[] {
            400,
            0,
            0,
            -2147483648});
            this.lj_x.Name = "lj_x";
            this.lj_x.Size = new System.Drawing.Size(120, 22);
            this.lj_x.TabIndex = 37;
            // 
            // lj_rotate
            // 
            this.lj_rotate.AutoSize = true;
            this.lj_rotate.Location = new System.Drawing.Point(13, 626);
            this.lj_rotate.Name = "lj_rotate";
            this.lj_rotate.Size = new System.Drawing.Size(72, 21);
            this.lj_rotate.TabIndex = 40;
            this.lj_rotate.Text = "Rotate";
            this.lj_rotate.UseVisualStyleBackColor = true;
            // 
            // lj_h
            // 
            this.lj_h.Location = new System.Drawing.Point(139, 530);
            this.lj_h.Maximum = new decimal(new int[] {
            3600,
            0,
            0,
            0});
            this.lj_h.Name = "lj_h";
            this.lj_h.Size = new System.Drawing.Size(120, 22);
            this.lj_h.TabIndex = 41;
            this.lj_h.Value = new decimal(new int[] {
            1500,
            0,
            0,
            0});
            // 
            // lj_f1
            // 
            this.lj_f1.Location = new System.Drawing.Point(13, 570);
            this.lj_f1.Maximum = new decimal(new int[] {
            400,
            0,
            0,
            0});
            this.lj_f1.Minimum = new decimal(new int[] {
            400,
            0,
            0,
            -2147483648});
            this.lj_f1.Name = "lj_f1";
            this.lj_f1.Size = new System.Drawing.Size(120, 22);
            this.lj_f1.TabIndex = 42;
            this.lj_f1.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // lj_f2
            // 
            this.lj_f2.Location = new System.Drawing.Point(13, 598);
            this.lj_f2.Maximum = new decimal(new int[] {
            400,
            0,
            0,
            0});
            this.lj_f2.Minimum = new decimal(new int[] {
            400,
            0,
            0,
            -2147483648});
            this.lj_f2.Name = "lj_f2";
            this.lj_f2.Size = new System.Drawing.Size(120, 22);
            this.lj_f2.TabIndex = 43;
            this.lj_f2.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lj_phi1
            // 
            this.lj_phi1.Location = new System.Drawing.Point(139, 570);
            this.lj_phi1.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.lj_phi1.Minimum = new decimal(new int[] {
            360,
            0,
            0,
            -2147483648});
            this.lj_phi1.Name = "lj_phi1";
            this.lj_phi1.Size = new System.Drawing.Size(120, 22);
            this.lj_phi1.TabIndex = 44;
            // 
            // lj_phi2
            // 
            this.lj_phi2.Location = new System.Drawing.Point(139, 598);
            this.lj_phi2.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.lj_phi2.Minimum = new decimal(new int[] {
            360,
            0,
            0,
            -2147483648});
            this.lj_phi2.Name = "lj_phi2";
            this.lj_phi2.Size = new System.Drawing.Size(120, 22);
            this.lj_phi2.TabIndex = 45;
            this.lj_phi2.Value = new decimal(new int[] {
            90,
            0,
            0,
            0});
            // 
            // btn_save
            // 
            this.btn_save.Location = new System.Drawing.Point(623, 269);
            this.btn_save.Name = "btn_save";
            this.btn_save.Size = new System.Drawing.Size(156, 23);
            this.btn_save.TabIndex = 47;
            this.btn_save.Text = "Save";
            this.btn_save.UseVisualStyleBackColor = true;
            this.btn_save.Click += new System.EventHandler(this.btn_save_Click);
            // 
            // svg_load
            // 
            this.svg_load.Location = new System.Drawing.Point(265, 667);
            this.svg_load.Name = "svg_load";
            this.svg_load.Size = new System.Drawing.Size(125, 26);
            this.svg_load.TabIndex = 48;
            this.svg_load.Text = "Import SVG";
            this.svg_load.UseVisualStyleBackColor = true;
            this.svg_load.Click += new System.EventHandler(this.svg_load_Click);
            // 
            // svg_y
            // 
            this.svg_y.Location = new System.Drawing.Point(13, 695);
            this.svg_y.Maximum = new decimal(new int[] {
            1800,
            0,
            0,
            0});
            this.svg_y.Minimum = new decimal(new int[] {
            1800,
            0,
            0,
            -2147483648});
            this.svg_y.Name = "svg_y";
            this.svg_y.Size = new System.Drawing.Size(120, 22);
            this.svg_y.TabIndex = 50;
            // 
            // svg_x
            // 
            this.svg_x.Location = new System.Drawing.Point(13, 667);
            this.svg_x.Maximum = new decimal(new int[] {
            400,
            0,
            0,
            0});
            this.svg_x.Minimum = new decimal(new int[] {
            400,
            0,
            0,
            -2147483648});
            this.svg_x.Name = "svg_x";
            this.svg_x.Size = new System.Drawing.Size(120, 22);
            this.svg_x.TabIndex = 49;
            // 
            // svg_w
            // 
            this.svg_w.Location = new System.Drawing.Point(139, 667);
            this.svg_w.Maximum = new decimal(new int[] {
            800,
            0,
            0,
            0});
            this.svg_w.Name = "svg_w";
            this.svg_w.Size = new System.Drawing.Size(120, 22);
            this.svg_w.TabIndex = 51;
            this.svg_w.Value = new decimal(new int[] {
            200,
            0,
            0,
            0});
            // 
            // operate_box
            // 
            this.operate_box.BackColor = System.Drawing.Color.White;
            this.operate_box.Location = new System.Drawing.Point(623, 403);
            this.operate_box.Name = "operate_box";
            this.operate_box.Size = new System.Drawing.Size(156, 500);
            this.operate_box.TabIndex = 52;
            this.operate_box.TabStop = false;
            // 
            // mainwin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(797, 922);
            this.Controls.Add(this.operate_box);
            this.Controls.Add(this.svg_w);
            this.Controls.Add(this.svg_y);
            this.Controls.Add(this.svg_x);
            this.Controls.Add(this.svg_load);
            this.Controls.Add(this.btn_save);
            this.Controls.Add(this.lj_phi2);
            this.Controls.Add(this.lj_phi1);
            this.Controls.Add(this.lj_f2);
            this.Controls.Add(this.lj_f1);
            this.Controls.Add(this.lj_h);
            this.Controls.Add(this.lj_rotate);
            this.Controls.Add(this.lj_w);
            this.Controls.Add(this.lj_y);
            this.Controls.Add(this.lj_x);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lj_draw);
            this.Controls.Add(this.cmd_clear);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.flw_phi);
            this.Controls.Add(this.flw_h);
            this.Controls.Add(this.flw_y);
            this.Controls.Add(this.flw_x);
            this.Controls.Add(this.flw_add);
            this.Controls.Add(this.pbox);
            this.Controls.Add(this.btn_load);
            this.Controls.Add(this.progress);
            this.Controls.Add(this.btn_run);
            this.Controls.Add(this.ENNBOX);
            this.Controls.Add(this.trackZ);
            this.Controls.Add(this.trackY);
            this.Controls.Add(this.trackX);
            this.Controls.Add(this.red0);
            this.Controls.Add(this.search_ftdi);
            this.Controls.Add(this.logbox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "mainwin";
            this.Text = "Egg Painter";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.mainwin_FormClosing);
            this.Load += new System.EventHandler(this.mainwin_Load);
            ((System.ComponentModel.ISupportInitialize)(this.trackX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackZ)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.flw_y)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.flw_x)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.flw_h)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.flw_phi)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lj_w)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lj_y)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lj_x)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lj_h)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lj_f1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lj_f2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lj_phi1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lj_phi2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.svg_y)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.svg_x)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.svg_w)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.operate_box)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox logbox;
        private System.Windows.Forms.Button search_ftdi;
        private System.Windows.Forms.Timer itime;
        private System.Windows.Forms.Button red0;
        private System.Windows.Forms.TrackBar trackX;
        private System.Windows.Forms.TrackBar trackY;
        private System.Windows.Forms.TrackBar trackZ;
        private System.Windows.Forms.CheckBox ENNBOX;
        private System.Windows.Forms.Button btn_run;
        private System.Windows.Forms.ProgressBar progress;
        private System.Windows.Forms.Button btn_load;
        private System.Windows.Forms.PictureBox pbox;
        private System.Windows.Forms.Button flw_add;
        private System.Windows.Forms.NumericUpDown flw_y;
        private System.Windows.Forms.NumericUpDown flw_x;
        private System.Windows.Forms.NumericUpDown flw_h;
        private System.Windows.Forms.NumericUpDown flw_phi;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button cmd_clear;
        private System.Windows.Forms.Button lj_draw;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown lj_w;
        private System.Windows.Forms.NumericUpDown lj_y;
        private System.Windows.Forms.NumericUpDown lj_x;
        private System.Windows.Forms.CheckBox lj_rotate;
        private System.Windows.Forms.NumericUpDown lj_h;
        private System.Windows.Forms.NumericUpDown lj_f1;
        private System.Windows.Forms.NumericUpDown lj_f2;
        private System.Windows.Forms.NumericUpDown lj_phi1;
        private System.Windows.Forms.NumericUpDown lj_phi2;
        private System.Windows.Forms.Button btn_save;
        private System.Windows.Forms.Button svg_load;
        private System.Windows.Forms.NumericUpDown svg_y;
        private System.Windows.Forms.NumericUpDown svg_x;
        private System.Windows.Forms.NumericUpDown svg_w;
        private System.Windows.Forms.PictureBox operate_box;
    }
}

