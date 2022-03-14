
namespace TrackerUI
{
    partial class TournamentViewerForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TournamentViewerForm));
            this.headerLabel = new System.Windows.Forms.Label();
            this.roundLabel = new System.Windows.Forms.Label();
            this.roundDropDown = new System.Windows.Forms.ComboBox();
            this.upcomingMatchOnlyCheckBox = new System.Windows.Forms.CheckBox();
            this.tournamentNameLabel = new System.Windows.Forms.Label();
            this.matchupListBox = new System.Windows.Forms.ListBox();
            this.team1Label = new System.Windows.Forms.Label();
            this.vsLabel = new System.Windows.Forms.Label();
            this.team2Label = new System.Windows.Forms.Label();
            this.team1ScoreTextBox = new System.Windows.Forms.TextBox();
            this.team2ScoreTextBox = new System.Windows.Forms.TextBox();
            this.saveScoreButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // headerLabel
            // 
            this.headerLabel.AutoSize = true;
            this.headerLabel.Font = new System.Drawing.Font("Segoe UI Light", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.headerLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(153)))), ((int)(((byte)(255)))));
            this.headerLabel.Location = new System.Drawing.Point(115, 9);
            this.headerLabel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.headerLabel.Name = "headerLabel";
            this.headerLabel.Size = new System.Drawing.Size(284, 62);
            this.headerLabel.TabIndex = 0;
            this.headerLabel.Text = "Tournament :";
            // 
            // roundLabel
            // 
            this.roundLabel.AutoSize = true;
            this.roundLabel.Font = new System.Drawing.Font("Segoe UI", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.roundLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(153)))), ((int)(((byte)(255)))));
            this.roundLabel.Location = new System.Drawing.Point(15, 83);
            this.roundLabel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.roundLabel.Name = "roundLabel";
            this.roundLabel.Size = new System.Drawing.Size(142, 46);
            this.roundLabel.TabIndex = 1;
            this.roundLabel.Text = "Round : ";
            // 
            // roundDropDown
            // 
            this.roundDropDown.FormattingEnabled = true;
            this.roundDropDown.Location = new System.Drawing.Point(166, 85);
            this.roundDropDown.Name = "roundDropDown";
            this.roundDropDown.Size = new System.Drawing.Size(54, 44);
            this.roundDropDown.TabIndex = 3;
            this.roundDropDown.Text = "1";
            // 
            // upcomingMatchOnlyCheckBox
            // 
            this.upcomingMatchOnlyCheckBox.AutoSize = true;
            this.upcomingMatchOnlyCheckBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(153)))), ((int)(((byte)(255)))));
            this.upcomingMatchOnlyCheckBox.Location = new System.Drawing.Point(23, 162);
            this.upcomingMatchOnlyCheckBox.Name = "upcomingMatchOnlyCheckBox";
            this.upcomingMatchOnlyCheckBox.Size = new System.Drawing.Size(306, 41);
            this.upcomingMatchOnlyCheckBox.TabIndex = 5;
            this.upcomingMatchOnlyCheckBox.Text = "Upcoming Match Only";
            this.upcomingMatchOnlyCheckBox.UseVisualStyleBackColor = true;
            // 
            // tournamentNameLabel
            // 
            this.tournamentNameLabel.AutoSize = true;
            this.tournamentNameLabel.Font = new System.Drawing.Font("Segoe UI Light", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tournamentNameLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(153)))), ((int)(((byte)(255)))));
            this.tournamentNameLabel.Location = new System.Drawing.Point(411, 12);
            this.tournamentNameLabel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.tournamentNameLabel.Name = "tournamentNameLabel";
            this.tournamentNameLabel.Size = new System.Drawing.Size(84, 62);
            this.tournamentNameLabel.TabIndex = 6;
            this.tournamentNameLabel.Text = "---";
            // 
            // matchupListBox
            // 
            this.matchupListBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.matchupListBox.FormattingEnabled = true;
            this.matchupListBox.ItemHeight = 36;
            this.matchupListBox.Location = new System.Drawing.Point(23, 225);
            this.matchupListBox.Name = "matchupListBox";
            this.matchupListBox.Size = new System.Drawing.Size(230, 182);
            this.matchupListBox.TabIndex = 7;
            // 
            // team1Label
            // 
            this.team1Label.AutoSize = true;
            this.team1Label.Font = new System.Drawing.Font("Segoe UI", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.team1Label.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(153)))), ((int)(((byte)(255)))));
            this.team1Label.Location = new System.Drawing.Point(364, 217);
            this.team1Label.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.team1Label.Name = "team1Label";
            this.team1Label.Size = new System.Drawing.Size(62, 46);
            this.team1Label.TabIndex = 8;
            this.team1Label.Text = "---";
            // 
            // vsLabel
            // 
            this.vsLabel.AutoSize = true;
            this.vsLabel.Font = new System.Drawing.Font("Segoe UI", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.vsLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(153)))), ((int)(((byte)(255)))));
            this.vsLabel.Location = new System.Drawing.Point(364, 294);
            this.vsLabel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.vsLabel.Name = "vsLabel";
            this.vsLabel.Size = new System.Drawing.Size(59, 46);
            this.vsLabel.TabIndex = 9;
            this.vsLabel.Text = "VS";
            // 
            // team2Label
            // 
            this.team2Label.AutoSize = true;
            this.team2Label.Font = new System.Drawing.Font("Segoe UI", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.team2Label.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(153)))), ((int)(((byte)(255)))));
            this.team2Label.Location = new System.Drawing.Point(364, 333);
            this.team2Label.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.team2Label.Name = "team2Label";
            this.team2Label.Size = new System.Drawing.Size(62, 46);
            this.team2Label.TabIndex = 10;
            this.team2Label.Text = "---";
            // 
            // team1ScoreTextBox
            // 
            this.team1ScoreTextBox.Location = new System.Drawing.Point(336, 248);
            this.team1ScoreTextBox.Name = "team1ScoreTextBox";
            this.team1ScoreTextBox.Size = new System.Drawing.Size(100, 42);
            this.team1ScoreTextBox.TabIndex = 11;
            // 
            // team2ScoreTextBox
            // 
            this.team2ScoreTextBox.Location = new System.Drawing.Point(336, 363);
            this.team2ScoreTextBox.Name = "team2ScoreTextBox";
            this.team2ScoreTextBox.Size = new System.Drawing.Size(100, 42);
            this.team2ScoreTextBox.TabIndex = 12;
            // 
            // saveScoreButton
            // 
            this.saveScoreButton.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.saveScoreButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.saveScoreButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this.saveScoreButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.saveScoreButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(153)))), ((int)(((byte)(255)))));
            this.saveScoreButton.Location = new System.Drawing.Point(472, 300);
            this.saveScoreButton.Name = "saveScoreButton";
            this.saveScoreButton.Size = new System.Drawing.Size(125, 39);
            this.saveScoreButton.TabIndex = 13;
            this.saveScoreButton.Text = "Save Score";
            this.saveScoreButton.UseVisualStyleBackColor = true;
            // 
            // TournamentViewerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(14F, 36F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(622, 433);
            this.Controls.Add(this.saveScoreButton);
            this.Controls.Add(this.team2ScoreTextBox);
            this.Controls.Add(this.team1ScoreTextBox);
            this.Controls.Add(this.team2Label);
            this.Controls.Add(this.vsLabel);
            this.Controls.Add(this.team1Label);
            this.Controls.Add(this.matchupListBox);
            this.Controls.Add(this.tournamentNameLabel);
            this.Controls.Add(this.upcomingMatchOnlyCheckBox);
            this.Controls.Add(this.roundDropDown);
            this.Controls.Add(this.roundLabel);
            this.Controls.Add(this.headerLabel);
            this.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            this.Name = "TournamentViewerForm";
            this.Text = "Tournament Viewer";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label headerLabel;
        private System.Windows.Forms.Label roundLabel;
        private System.Windows.Forms.ComboBox roundDropDown;
        private System.Windows.Forms.CheckBox upcomingMatchOnlyCheckBox;
        private System.Windows.Forms.Label tournamentNameLabel;
        private System.Windows.Forms.ListBox matchupListBox;
        private System.Windows.Forms.Label team1Label;
        private System.Windows.Forms.Label vsLabel;
        private System.Windows.Forms.Label team2Label;
        private System.Windows.Forms.TextBox team1ScoreTextBox;
        private System.Windows.Forms.TextBox team2ScoreTextBox;
        private System.Windows.Forms.Button saveScoreButton;
    }
}

