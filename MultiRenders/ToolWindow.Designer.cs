namespace MultiRenders
{
    partial class ToolWindow
    {
        private System.ComponentModel.IContainer components = null;

        public System.Windows.Forms.RadioButton radioButtonColorByPosition;
        public System.Windows.Forms.RadioButton radioButtonDynamicLightSpecular;
        public System.Windows.Forms.RadioButton radioButtonMoveCubeToSphere;
        public System.Windows.Forms.Button buttonResetLightPosition;


        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            radioButtonColorByPosition = new System.Windows.Forms.RadioButton();
            radioButtonDynamicLightSpecular = new System.Windows.Forms.RadioButton();
            radioButtonMoveCubeToSphere = new System.Windows.Forms.RadioButton();
            buttonResetLightPosition = new System.Windows.Forms.Button();
            SuspendLayout();
            // 
            // radioButtonColorByPosition
            // 
            radioButtonColorByPosition.Checked = true;
            radioButtonColorByPosition.Location = new System.Drawing.Point(20, 30);
            radioButtonColorByPosition.Name = "radioButtonColorByPosition";
            radioButtonColorByPosition.Size = new System.Drawing.Size(150, 20);
            radioButtonColorByPosition.TabIndex = 0;
            radioButtonColorByPosition.TabStop = true;
            radioButtonColorByPosition.Text = "Color By Position";
            radioButtonColorByPosition.CheckedChanged += radioButtonColorByPosition_CheckedChanged;
            // 
            // radioButtonDynamicLightSpecular
            // 
            radioButtonDynamicLightSpecular.Location = new System.Drawing.Point(20, 80);
            radioButtonDynamicLightSpecular.Name = "radioButtonDynamicLightSpecular";
            radioButtonDynamicLightSpecular.Size = new System.Drawing.Size(150, 20);
            radioButtonDynamicLightSpecular.TabIndex = 1;
            radioButtonDynamicLightSpecular.Text = "Dynamic Light Specular";
            radioButtonDynamicLightSpecular.CheckedChanged += radioButtonDynamicLightSpecular_CheckedChanged;
            // 
            // radioButtonMoveCubeToSphere
            // 
            radioButtonMoveCubeToSphere.Location = new System.Drawing.Point(20, 150);
            radioButtonMoveCubeToSphere.Name = "radioButtonMoveCubeToSphere";
            radioButtonMoveCubeToSphere.Size = new System.Drawing.Size(150, 20);
            radioButtonMoveCubeToSphere.TabIndex = 2;
            radioButtonMoveCubeToSphere.Text = "Move Cube To Sphere";
            radioButtonMoveCubeToSphere.CheckedChanged += radioButtonMoveCubeToSphere_CheckedChanged;
            // 
            // buttonResetLightPosition
            // 
            buttonResetLightPosition.Location = new System.Drawing.Point(40, 100);
            buttonResetLightPosition.Name = "buttonResetLightPosition";
            buttonResetLightPosition.Size = new System.Drawing.Size(150, 30);
            buttonResetLightPosition.TabIndex = 3;
            buttonResetLightPosition.Text = "Reset Light Position";
            buttonResetLightPosition.Click += buttonResetLightPosition_Click;
            // 
            // ToolWindow
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(250, 200);
            Controls.Add(radioButtonColorByPosition);
            Controls.Add(radioButtonDynamicLightSpecular);
            Controls.Add(radioButtonMoveCubeToSphere);
            Controls.Add(buttonResetLightPosition);
            Name = "ToolWindow";
            Text = "ToolWindow";
            ResumeLayout(false);
        }
    }
}