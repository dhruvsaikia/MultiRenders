using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MultiRenders
{
    public partial class ToolWindow : Form
    {
        public event EventHandler<EventArgs> ModeChanged;
        public bool RadioButtonColorByPositionChecked => radioButtonColorByPosition.Checked;
        public bool RadioButtonDynamicLightSpecularChecked => radioButtonDynamicLightSpecular.Checked;

        public ToolWindow()
        {
            InitializeComponent();
            radioButtonColorByPosition.Checked = true;
        }

        private void radioButtonColorByPosition_CheckedChanged(object sender, EventArgs e)
        {
            OnModeChanged();
        }

        private void radioButtonDynamicLightSpecular_CheckedChanged(object sender, EventArgs e)
        {
            OnModeChanged();
        }

        private void buttonResetLightPosition_Click(object sender, EventArgs e)
        {

        }

        private void radioButtonMoveCubeToSphere_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void OnModeChanged()
        {
            if (radioButtonColorByPosition.Checked)
            {
                ModeChanged?.Invoke(this, EventArgs.Empty);
            }
            else if (radioButtonDynamicLightSpecular.Checked)
            {
                ModeChanged?.Invoke(this, EventArgs.Empty);
            }
            // Add logic for other radio buttons if needed
        }
    }
}
