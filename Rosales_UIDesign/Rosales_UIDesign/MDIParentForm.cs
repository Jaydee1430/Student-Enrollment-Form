using System;
using System.Drawing;
using System.Windows.Forms;

namespace Rosales_UIDesign
{
    public partial class MDIParentForm : Form
    {
        public MDIParentForm()
        {
            InitializeComponent();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void enrollmentFormToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form child in this.MdiChildren)
            {
                child.Close();
            }
            FrmEnrollment enrollForm = new FrmEnrollment();
            enrollForm.MdiParent = this;
            enrollForm.StartPosition = FormStartPosition.Manual;
            enrollForm.Location = new Point(0, 0);
            enrollForm.Show();
            this.ClientSize = enrollForm.Size;
        }

        private void MDIParentForm_Load(object sender, EventArgs e)
        {

        }

        private void studToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form child in this.MdiChildren)
            {
                child.Close();
            }
            FrmStudentView stuList = new FrmStudentView();
            stuList.MdiParent = this;
            stuList.StartPosition = FormStartPosition.Manual;
            stuList.Location = new Point(0, 0);
            stuList.Show();
            this.ClientSize = stuList.Size;
        }
    }
}
