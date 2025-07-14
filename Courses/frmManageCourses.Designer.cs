namespace The_Story_Corner_Project.Courses
{
    partial class frmManageCourses
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmManageCourses));
            this.kryptonPalette1 = new ComponentFactory.Krypton.Toolkit.KryptonPalette(this.components);
            this.label4 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.lblRecordsCount = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnAddBook = new System.Windows.Forms.Button();
            this.dtpMaxDate = new System.Windows.Forms.DateTimePicker();
            this.dtpMinStartDate = new System.Windows.Forms.DateTimePicker();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.pnlStatus = new System.Windows.Forms.Panel();
            this.rbUpComing = new System.Windows.Forms.RadioButton();
            this.rbExpired = new System.Windows.Forms.RadioButton();
            this.rbActive = new System.Windows.Forms.RadioButton();
            this.cbFilterBy = new System.Windows.Forms.ComboBox();
            this.txtFilterValue = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.pctrLoading = new System.Windows.Forms.PictureBox();
            this.dgvCourses = new System.Windows.Forms.DataGridView();
            this.cmsBorrows = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.AddNewCourseToolItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.showDetailsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.EnrollParicipantToolItem = new System.Windows.Forms.ToolStripMenuItem();
            this.listParticipantsItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel4.SuspendLayout();
            this.panel2.SuspendLayout();
            this.pnlStatus.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pctrLoading)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCourses)).BeginInit();
            this.cmsBorrows.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // kryptonPalette1
            // 
            this.kryptonPalette1.ButtonStyles.ButtonCommon.StateCommon.Back.Color1 = System.Drawing.Color.White;
            this.kryptonPalette1.ButtonStyles.ButtonCommon.StateCommon.Back.Color2 = System.Drawing.Color.White;
            this.kryptonPalette1.ButtonStyles.ButtonCommon.StateCommon.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.kryptonPalette1.ButtonStyles.ButtonCommon.StateCommon.Border.Width = 0;
            this.kryptonPalette1.ButtonStyles.ButtonCommon.StateNormal.Back.Color1 = System.Drawing.Color.White;
            this.kryptonPalette1.ButtonStyles.ButtonCommon.StateNormal.Back.Color2 = System.Drawing.Color.White;
            this.kryptonPalette1.ButtonStyles.ButtonCommon.StateNormal.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.kryptonPalette1.ButtonStyles.ButtonCommon.StateNormal.Border.Width = 0;
            this.kryptonPalette1.ButtonStyles.ButtonCommon.StatePressed.Back.Color1 = System.Drawing.Color.White;
            this.kryptonPalette1.ButtonStyles.ButtonCommon.StatePressed.Back.Color2 = System.Drawing.Color.White;
            this.kryptonPalette1.ButtonStyles.ButtonCommon.StatePressed.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.kryptonPalette1.ButtonStyles.ButtonCommon.StatePressed.Border.Width = 0;
            this.kryptonPalette1.ButtonStyles.ButtonCommon.StateTracking.Back.Color1 = System.Drawing.Color.White;
            this.kryptonPalette1.ButtonStyles.ButtonCommon.StateTracking.Back.Color2 = System.Drawing.Color.White;
            this.kryptonPalette1.ButtonStyles.ButtonCommon.StateTracking.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.kryptonPalette1.ButtonStyles.ButtonCommon.StateTracking.Border.Width = 0;
            this.kryptonPalette1.FormStyles.FormMain.StateCommon.Back.Color1 = System.Drawing.Color.White;
            this.kryptonPalette1.FormStyles.FormMain.StateCommon.Back.Color2 = System.Drawing.Color.White;
            this.kryptonPalette1.FormStyles.FormMain.StateCommon.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.kryptonPalette1.FormStyles.FormMain.StateCommon.Border.GraphicsHint = ComponentFactory.Krypton.Toolkit.PaletteGraphicsHint.None;
            this.kryptonPalette1.FormStyles.FormMain.StateCommon.Border.Rounding = 15;
            this.kryptonPalette1.HeaderStyles.HeaderCommon.StateCommon.Back.Color1 = System.Drawing.Color.White;
            this.kryptonPalette1.HeaderStyles.HeaderCommon.StateCommon.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.kryptonPalette1.HeaderStyles.HeaderCommon.StateCommon.Content.Padding = new System.Windows.Forms.Padding(10, -1, -1, -1);
            this.kryptonPalette1.HeaderStyles.HeaderForm.StateCommon.ButtonEdgeInset = 10;
            // 
            // label4
            // 
            this.label4.Dock = System.Windows.Forms.DockStyle.Top;
            this.label4.Font = new System.Drawing.Font("Poppins Medium", 25.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(51)))), ((int)(((byte)(102)))));
            this.label4.Location = new System.Drawing.Point(7, 126);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(1868, 65);
            this.label4.TabIndex = 99;
            this.label4.Text = "Courses management";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.lblRecordsCount);
            this.panel4.Controls.Add(this.label1);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(7, 700);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(1868, 108);
            this.panel4.TabIndex = 100;
            // 
            // lblRecordsCount
            // 
            this.lblRecordsCount.AutoSize = true;
            this.lblRecordsCount.Font = new System.Drawing.Font("Poppins", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRecordsCount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(51)))), ((int)(((byte)(102)))));
            this.lblRecordsCount.Location = new System.Drawing.Point(214, 6);
            this.lblRecordsCount.Name = "lblRecordsCount";
            this.lblRecordsCount.Size = new System.Drawing.Size(56, 36);
            this.lblRecordsCount.TabIndex = 95;
            this.lblRecordsCount.Text = "? ? ?";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Poppins", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(51)))), ((int)(((byte)(102)))));
            this.label1.Location = new System.Drawing.Point(36, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(172, 36);
            this.label1.TabIndex = 94;
            this.label1.Text = "Records count :";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnAddBook);
            this.panel2.Controls.Add(this.dtpMaxDate);
            this.panel2.Controls.Add(this.dtpMinStartDate);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.label6);
            this.panel2.Controls.Add(this.pnlStatus);
            this.panel2.Controls.Add(this.cbFilterBy);
            this.panel2.Controls.Add(this.txtFilterValue);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(7, 191);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1868, 82);
            this.panel2.TabIndex = 101;
            // 
            // btnAddBook
            // 
            this.btnAddBook.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddBook.Image = global::The_Story_Corner_Project.Properties.Resources._6586117_add_create_new_plus_icon__1_;
            this.btnAddBook.Location = new System.Drawing.Point(1675, 14);
            this.btnAddBook.Name = "btnAddBook";
            this.btnAddBook.Size = new System.Drawing.Size(98, 51);
            this.btnAddBook.TabIndex = 103;
            this.btnAddBook.UseVisualStyleBackColor = true;
            this.btnAddBook.Click += new System.EventHandler(this.btnAddBook_Click_1);
            // 
            // dtpMaxDate
            // 
            this.dtpMaxDate.CustomFormat = "dd/M/yyyy";
            this.dtpMaxDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpMaxDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpMaxDate.Location = new System.Drawing.Point(1441, 26);
            this.dtpMaxDate.MinDate = new System.DateTime(1900, 1, 1, 0, 0, 0, 0);
            this.dtpMaxDate.Name = "dtpMaxDate";
            this.dtpMaxDate.Size = new System.Drawing.Size(157, 27);
            this.dtpMaxDate.TabIndex = 165;
            this.dtpMaxDate.Value = new System.DateTime(2000, 12, 31, 0, 0, 0, 0);
            this.dtpMaxDate.ValueChanged += new System.EventHandler(this.dtpEndDate_ValueChanged);
            // 
            // dtpMinStartDate
            // 
            this.dtpMinStartDate.CustomFormat = "dd/M/yyyy";
            this.dtpMinStartDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpMinStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpMinStartDate.Location = new System.Drawing.Point(1215, 26);
            this.dtpMinStartDate.MinDate = new System.DateTime(1900, 1, 1, 0, 0, 0, 0);
            this.dtpMinStartDate.Name = "dtpMinStartDate";
            this.dtpMinStartDate.Size = new System.Drawing.Size(157, 27);
            this.dtpMinStartDate.TabIndex = 166;
            this.dtpMinStartDate.Value = new System.DateTime(2000, 12, 31, 0, 0, 0, 0);
            this.dtpMinStartDate.ValueChanged += new System.EventHandler(this.dtpStartDate_ValueChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Poppins", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(51)))), ((int)(((byte)(102)))));
            this.label5.Location = new System.Drawing.Point(1378, 21);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(57, 36);
            this.label5.TabIndex = 164;
            this.label5.Text = "And";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Poppins", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(51)))), ((int)(((byte)(102)))));
            this.label6.Location = new System.Drawing.Point(950, 25);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(266, 36);
            this.label6.TabIndex = 163;
            this.label6.Text = "Courses Starts Between: ";
            // 
            // pnlStatus
            // 
            this.pnlStatus.Controls.Add(this.rbUpComing);
            this.pnlStatus.Controls.Add(this.rbExpired);
            this.pnlStatus.Controls.Add(this.rbActive);
            this.pnlStatus.Location = new System.Drawing.Point(379, 19);
            this.pnlStatus.Name = "pnlStatus";
            this.pnlStatus.Size = new System.Drawing.Size(411, 42);
            this.pnlStatus.TabIndex = 101;
            this.pnlStatus.Visible = false;
            // 
            // rbUpComing
            // 
            this.rbUpComing.AutoSize = true;
            this.rbUpComing.Checked = true;
            this.rbUpComing.Font = new System.Drawing.Font("Poppins", 10F);
            this.rbUpComing.Location = new System.Drawing.Point(5, 4);
            this.rbUpComing.Name = "rbUpComing";
            this.rbUpComing.Size = new System.Drawing.Size(121, 34);
            this.rbUpComing.TabIndex = 3;
            this.rbUpComing.TabStop = true;
            this.rbUpComing.Text = "Upcoming";
            this.rbUpComing.UseVisualStyleBackColor = true;
            this.rbUpComing.CheckedChanged += new System.EventHandler(this.rbUpComing_CheckedChanged);
            // 
            // rbExpired
            // 
            this.rbExpired.AutoSize = true;
            this.rbExpired.Font = new System.Drawing.Font("Poppins", 10F);
            this.rbExpired.Location = new System.Drawing.Point(296, 4);
            this.rbExpired.Name = "rbExpired";
            this.rbExpired.Size = new System.Drawing.Size(94, 34);
            this.rbExpired.TabIndex = 2;
            this.rbExpired.Text = "Expired";
            this.rbExpired.UseVisualStyleBackColor = true;
            this.rbExpired.CheckedChanged += new System.EventHandler(this.rbFemale_CheckedChanged);
            // 
            // rbActive
            // 
            this.rbActive.AutoSize = true;
            this.rbActive.Font = new System.Drawing.Font("Poppins", 10F);
            this.rbActive.Location = new System.Drawing.Point(158, 4);
            this.rbActive.Name = "rbActive";
            this.rbActive.Size = new System.Drawing.Size(106, 34);
            this.rbActive.TabIndex = 2;
            this.rbActive.Text = "Ongoing";
            this.rbActive.UseVisualStyleBackColor = true;
            this.rbActive.CheckedChanged += new System.EventHandler(this.rbActive_CheckedChanged);
            // 
            // cbFilterBy
            // 
            this.cbFilterBy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbFilterBy.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.cbFilterBy.FormattingEnabled = true;
            this.cbFilterBy.Items.AddRange(new object[] {
            "None",
            "Status",
            "course name",
            "Tutor name"});
            this.cbFilterBy.Location = new System.Drawing.Point(156, 26);
            this.cbFilterBy.Name = "cbFilterBy";
            this.cbFilterBy.Size = new System.Drawing.Size(210, 28);
            this.cbFilterBy.TabIndex = 100;
            this.cbFilterBy.SelectedIndexChanged += new System.EventHandler(this.cbFilterBy_SelectedIndexChanged);
            // 
            // txtFilterValue
            // 
            this.txtFilterValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtFilterValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.txtFilterValue.Location = new System.Drawing.Point(373, 27);
            this.txtFilterValue.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtFilterValue.Name = "txtFilterValue";
            this.txtFilterValue.Size = new System.Drawing.Size(256, 26);
            this.txtFilterValue.TabIndex = 99;
            this.txtFilterValue.TextChanged += new System.EventHandler(this.txtFilterValue_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Poppins", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(51)))), ((int)(((byte)(102)))));
            this.label3.Location = new System.Drawing.Point(36, 21);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(116, 36);
            this.label3.TabIndex = 98;
            this.label3.Text = "Filter by :  ";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.pctrLoading);
            this.panel3.Controls.Add(this.dgvCourses);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(7, 273);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1868, 427);
            this.panel3.TabIndex = 102;
            // 
            // pctrLoading
            // 
            this.pctrLoading.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pctrLoading.BackColor = System.Drawing.Color.White;
            this.pctrLoading.Image = global::The_Story_Corner_Project.Properties.Resources.undraw_Loading_re_5axr;
            this.pctrLoading.Location = new System.Drawing.Point(677, 81);
            this.pctrLoading.Name = "pctrLoading";
            this.pctrLoading.Size = new System.Drawing.Size(512, 307);
            this.pctrLoading.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pctrLoading.TabIndex = 104;
            this.pctrLoading.TabStop = false;
            this.pctrLoading.Visible = false;
            // 
            // dgvCourses
            // 
            this.dgvCourses.AllowUserToAddRows = false;
            this.dgvCourses.AllowUserToDeleteRows = false;
            this.dgvCourses.AllowUserToOrderColumns = true;
            this.dgvCourses.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvCourses.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.Padding = new System.Windows.Forms.Padding(10, 0, 10, 0);
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvCourses.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvCourses.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCourses.ContextMenuStrip = this.cmsBorrows;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Poppins", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.Padding = new System.Windows.Forms.Padding(4);
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvCourses.DefaultCellStyle = dataGridViewCellStyle4;
            this.dgvCourses.Location = new System.Drawing.Point(85, 11);
            this.dgvCourses.Margin = new System.Windows.Forms.Padding(200, 3, 200, 3);
            this.dgvCourses.Name = "dgvCourses";
            this.dgvCourses.ReadOnly = true;
            this.dgvCourses.RowHeadersWidth = 51;
            this.dgvCourses.RowTemplate.Height = 24;
            this.dgvCourses.Size = new System.Drawing.Size(1689, 397);
            this.dgvCourses.TabIndex = 0;
            this.dgvCourses.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvCourses_CellMouseDown);
            // 
            // cmsBorrows
            // 
            this.cmsBorrows.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cmsBorrows.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.cmsBorrows.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AddNewCourseToolItem,
            this.toolStripSeparator1,
            this.showDetailsToolStripMenuItem,
            this.toolStripSeparator2,
            this.EnrollParicipantToolItem,
            this.listParticipantsItem,
            this.editToolStripMenuItem});
            this.cmsBorrows.Name = "contextMenuStrip1";
            this.cmsBorrows.Size = new System.Drawing.Size(270, 206);
            // 
            // AddNewCourseToolItem
            // 
            this.AddNewCourseToolItem.Font = new System.Drawing.Font("Poppins", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AddNewCourseToolItem.Image = global::The_Story_Corner_Project.Properties.Resources._2124277_add_essential_app_document_icon;
            this.AddNewCourseToolItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.AddNewCourseToolItem.Name = "AddNewCourseToolItem";
            this.AddNewCourseToolItem.Size = new System.Drawing.Size(269, 38);
            this.AddNewCourseToolItem.Text = "Launch new course";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(266, 6);
            // 
            // showDetailsToolStripMenuItem
            // 
            this.showDetailsToolStripMenuItem.Font = new System.Drawing.Font("Poppins", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.showDetailsToolStripMenuItem.Image = global::The_Story_Corner_Project.Properties.Resources.Question_32;
            this.showDetailsToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.showDetailsToolStripMenuItem.Name = "showDetailsToolStripMenuItem";
            this.showDetailsToolStripMenuItem.Size = new System.Drawing.Size(269, 38);
            this.showDetailsToolStripMenuItem.Text = "&Show course Details";
            this.showDetailsToolStripMenuItem.Click += new System.EventHandler(this.showDetailsToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(266, 6);
            // 
            // EnrollParicipantToolItem
            // 
            this.EnrollParicipantToolItem.Font = new System.Drawing.Font("Poppins", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.EnrollParicipantToolItem.Image = ((System.Drawing.Image)(resources.GetObject("EnrollParicipantToolItem.Image")));
            this.EnrollParicipantToolItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.EnrollParicipantToolItem.Name = "EnrollParicipantToolItem";
            this.EnrollParicipantToolItem.Size = new System.Drawing.Size(269, 38);
            this.EnrollParicipantToolItem.Text = "Enroll a participant";
            this.EnrollParicipantToolItem.Click += new System.EventHandler(this.EnroleParticipant_Click);
            // 
            // listParticipantsItem
            // 
            this.listParticipantsItem.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.listParticipantsItem.Font = new System.Drawing.Font("Poppins", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listParticipantsItem.Image = global::The_Story_Corner_Project.Properties.Resources._4168571_renew_refresh_reload_repeat_sync_icon__1_;
            this.listParticipantsItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.listParticipantsItem.Name = "listParticipantsItem";
            this.listParticipantsItem.Size = new System.Drawing.Size(269, 38);
            this.listParticipantsItem.Text = "List participants";
            this.listParticipantsItem.Click += new System.EventHandler(this.listParticipantsItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.Font = new System.Drawing.Font("Poppins", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.editToolStripMenuItem.Image = global::The_Story_Corner_Project.Properties.Resources.edit_32;
            this.editToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(269, 38);
            this.editToolStripMenuItem.Text = "&Edit";
            this.editToolStripMenuItem.Click += new System.EventHandler(this.editToolStripMenuItem_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.pictureBox1.Image = global::The_Story_Corner_Project.Properties.Resources._6771586_book_courses_education_learning_school_icon__1_;
            this.pictureBox1.Location = new System.Drawing.Point(7, 7);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(1868, 119);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 98;
            this.pictureBox1.TabStop = false;
            // 
            // frmManageCourses
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1882, 815);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmManageCourses";
            this.Padding = new System.Windows.Forms.Padding(7);
            this.Palette = this.kryptonPalette1;
            this.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.Custom;
            this.Text = "frmManageCourses";
            this.Load += new System.EventHandler(this.frmManageCourses_Load);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.pnlStatus.ResumeLayout(false);
            this.pnlStatus.PerformLayout();
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pctrLoading)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCourses)).EndInit();
            this.cmsBorrows.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private ComponentFactory.Krypton.Toolkit.KryptonPalette kryptonPalette1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label lblRecordsCount;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.PictureBox pctrLoading;
        private System.Windows.Forms.DataGridView dgvCourses;
        private System.Windows.Forms.ContextMenuStrip cmsBorrows;
        private System.Windows.Forms.ToolStripMenuItem AddNewCourseToolItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem showDetailsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem EnrollParicipantToolItem;
        private System.Windows.Forms.ToolStripMenuItem listParticipantsItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ComboBox cbFilterBy;
        private System.Windows.Forms.TextBox txtFilterValue;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel pnlStatus;
        private System.Windows.Forms.RadioButton rbExpired;
        private System.Windows.Forms.RadioButton rbActive;
        private System.Windows.Forms.DateTimePicker dtpMaxDate;
        private System.Windows.Forms.DateTimePicker dtpMinStartDate;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.RadioButton rbUpComing;
        private System.Windows.Forms.Button btnAddBook;
    }
}