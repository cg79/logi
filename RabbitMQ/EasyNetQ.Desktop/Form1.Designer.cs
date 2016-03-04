namespace EasyNetQ.Desktop
{
    partial class Form1
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
            this.button1 = new System.Windows.Forms.Button();
            this.txtQueueName = new System.Windows.Forms.TextBox();
            this.txtExchangeName = new System.Windows.Forms.TextBox();
            this.txtRoutingKey = new System.Windows.Forms.TextBox();
            this.txtExchangeType = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.button3 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.button4 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.chkDurable1 = new System.Windows.Forms.CheckBox();
            this.chkAutoDelete1 = new System.Windows.Forms.CheckBox();
            this.chkDurable2 = new System.Windows.Forms.CheckBox();
            this.chkAutoDelete2 = new System.Windows.Forms.CheckBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(63, 250);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(170, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Start Producer";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // txtQueueName
            // 
            this.txtQueueName.Location = new System.Drawing.Point(324, 281);
            this.txtQueueName.Name = "txtQueueName";
            this.txtQueueName.Size = new System.Drawing.Size(95, 22);
            this.txtQueueName.TabIndex = 1;
            this.txtQueueName.Text = "q1";
            // 
            // txtExchangeName
            // 
            this.txtExchangeName.Location = new System.Drawing.Point(135, 39);
            this.txtExchangeName.Name = "txtExchangeName";
            this.txtExchangeName.Size = new System.Drawing.Size(284, 22);
            this.txtExchangeName.TabIndex = 2;
            this.txtExchangeName.Text = "ex_direct";
            // 
            // txtRoutingKey
            // 
            this.txtRoutingKey.Location = new System.Drawing.Point(72, 156);
            this.txtRoutingKey.Name = "txtRoutingKey";
            this.txtRoutingKey.Size = new System.Drawing.Size(347, 22);
            this.txtRoutingKey.TabIndex = 3;
            this.txtRoutingKey.Text = "a";
            // 
            // txtExchangeType
            // 
            this.txtExchangeType.Location = new System.Drawing.Point(135, 83);
            this.txtExchangeType.Name = "txtExchangeType";
            this.txtExchangeType.Size = new System.Drawing.Size(284, 22);
            this.txtExchangeType.TabIndex = 4;
            this.txtExchangeType.Text = "topic";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(263, 250);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(156, 23);
            this.button2.TabIndex = 5;
            this.button2.Text = "Start Consumer";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Location = new System.Drawing.Point(263, 308);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(72, 21);
            this.checkBox1.TabIndex = 6;
            this.checkBox1.Text = "New Q";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(63, 349);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(791, 23);
            this.button3.TabIndex = 7;
            this.button3.Text = "Consumer Producer ";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(570, 83);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(284, 22);
            this.textBox1.TabIndex = 11;
            this.textBox1.Text = "t";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(507, 156);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(347, 22);
            this.textBox2.TabIndex = 10;
            this.textBox2.Text = "a";
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(570, 39);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(284, 22);
            this.textBox3.TabIndex = 9;
            this.textBox3.Text = "ex_direct1";
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(550, 278);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(244, 22);
            this.textBox4.TabIndex = 8;
            this.textBox4.Text = "q1";
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Checked = true;
            this.checkBox2.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox2.Location = new System.Drawing.Point(489, 308);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(72, 21);
            this.checkBox2.TabIndex = 13;
            this.checkBox2.Text = "New Q";
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(489, 250);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(365, 23);
            this.button4.TabIndex = 12;
            this.button4.Text = "Start Consumer";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(260, 281);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 17);
            this.label1.TabIndex = 14;
            this.label1.Text = "Q name";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(486, 281);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 17);
            this.label2.TabIndex = 15;
            this.label2.Text = "Q name";
            // 
            // chkDurable1
            // 
            this.chkDurable1.AutoSize = true;
            this.chkDurable1.Checked = true;
            this.chkDurable1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkDurable1.Location = new System.Drawing.Point(72, 185);
            this.chkDurable1.Name = "chkDurable1";
            this.chkDurable1.Size = new System.Drawing.Size(80, 21);
            this.chkDurable1.TabIndex = 16;
            this.chkDurable1.Text = "Durable";
            this.chkDurable1.UseVisualStyleBackColor = true;
            // 
            // chkAutoDelete1
            // 
            this.chkAutoDelete1.AutoSize = true;
            this.chkAutoDelete1.Checked = true;
            this.chkAutoDelete1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAutoDelete1.Location = new System.Drawing.Point(324, 184);
            this.chkAutoDelete1.Name = "chkAutoDelete1";
            this.chkAutoDelete1.Size = new System.Drawing.Size(100, 21);
            this.chkAutoDelete1.TabIndex = 17;
            this.chkAutoDelete1.Text = "AutoDelete";
            this.chkAutoDelete1.UseVisualStyleBackColor = true;
            // 
            // chkDurable2
            // 
            this.chkDurable2.AutoSize = true;
            this.chkDurable2.Checked = true;
            this.chkDurable2.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkDurable2.Location = new System.Drawing.Point(507, 185);
            this.chkDurable2.Name = "chkDurable2";
            this.chkDurable2.Size = new System.Drawing.Size(80, 21);
            this.chkDurable2.TabIndex = 18;
            this.chkDurable2.Text = "Durable";
            this.chkDurable2.UseVisualStyleBackColor = true;
            // 
            // chkAutoDelete2
            // 
            this.chkAutoDelete2.AutoSize = true;
            this.chkAutoDelete2.Checked = true;
            this.chkAutoDelete2.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAutoDelete2.Location = new System.Drawing.Point(756, 185);
            this.chkAutoDelete2.Name = "chkAutoDelete2";
            this.chkAutoDelete2.Size = new System.Drawing.Size(100, 21);
            this.chkAutoDelete2.TabIndex = 19;
            this.chkAutoDelete2.Text = "AutoDelete";
            this.chkAutoDelete2.UseVisualStyleBackColor = true;
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "fanout",
            "direct",
            "topic",
            "header"});
            this.comboBox1.Location = new System.Drawing.Point(135, 112);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 24);
            this.comboBox1.TabIndex = 20;
            // 
            // comboBox2
            // 
            this.comboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Items.AddRange(new object[] {
            "fanout",
            "direct",
            "topic",
            "header"});
            this.comboBox2.Location = new System.Drawing.Point(570, 110);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(121, 24);
            this.comboBox2.TabIndex = 21;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1052, 484);
            this.Controls.Add(this.comboBox2);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.chkAutoDelete2);
            this.Controls.Add(this.chkDurable2);
            this.Controls.Add(this.chkAutoDelete1);
            this.Controls.Add(this.chkDurable1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.checkBox2);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.textBox4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.txtExchangeType);
            this.Controls.Add(this.txtRoutingKey);
            this.Controls.Add(this.txtExchangeName);
            this.Controls.Add(this.txtQueueName);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox txtQueueName;
        private System.Windows.Forms.TextBox txtExchangeName;
        private System.Windows.Forms.TextBox txtRoutingKey;
        private System.Windows.Forms.TextBox txtExchangeType;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chkDurable1;
        private System.Windows.Forms.CheckBox chkAutoDelete1;
        private System.Windows.Forms.CheckBox chkDurable2;
        private System.Windows.Forms.CheckBox chkAutoDelete2;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.ComboBox comboBox2;
    }
}

