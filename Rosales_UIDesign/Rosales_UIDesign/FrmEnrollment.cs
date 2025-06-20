
using System;
using System.Collections;
using System.Windows.Forms;

namespace Rosales_UIDesign
{
    public partial class FrmEnrollment : Form
    {
        ArrayList programList = new ArrayList();
        private string connectionString = "Data Source=JAYDEE\\SQLEXPRESS;Initial Catalog=StudentRegDB;Integrated Security=True;" + "TrustServerCertificate=True";
        StudentController studentController;
        public FrmEnrollment()
        {
            InitializeComponent();
            StudentRepository repo = new StudentRepository(connectionString);
            studentController = new StudentController(repo);
        }
        private void FrmEnrollment_Load(object sender, EventArgs e)
        {   
            programList = studentController.GetAllPrograms();

            cmbProgram.DisplayMember = "ProgramName"; 
            cmbProgram.ValueMember = "TuitionFee"; 
            cmbProgram.DataSource = null;
            cmbProgram.DataSource = programList;

            ClearAllInputs(this);
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
              
              
            try
            {
                Guardian guardian = new Guardian(0, txtGuardianFirstName.Text, txtGuardianMiddleName.Text,
                                            txtGuardianLastName.Text, txtRelationship.Text,
                                            txtGuardianAddress.Text, txtGuardianContact.Text);

                Requirements requirements = new Requirements();
                requirements.BirthCertificate = chkBirthCertificate.Checked;
                requirements.TOR = chkTranscript.Checked;
                requirements.GoodMoral = chkGoodMoral.Checked;

                ProgramInfo programInfo = new ProgramInfo(0, cmbProgram.Text,
                                              cmbProgram.SelectedValue != null ? (decimal)cmbProgram.SelectedValue : 0);

                string modeOfPayment = rbCash.Checked ? rbCash.Text : rbInstallment.Text;
                Payment payment = new Payment(0, decimal.Parse(txtAmountPay.Text), modeOfPayment, GetDiscount());

                Student student = new Student(lblStudNumber.Text, txtFirstName.Text, txtLastName.Text,
                                            txtMiddleName.Text, dtpDOB.Value, programInfo, guardian,
                                            requirements, payment, txtContact.Text, txtAddress.Text,
                                            cmbYearLevel.Text);
                studentController.SaveStudentRegistration(student);
                MessageBox.Show(student.DisplayInfo(), "Student Registered", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex) { 
                MessageBox.Show(ex.Message, "Student Registered", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearAllInputs(this);
            lblStudNumber.Text = IDGenerator.GenerateStudentID();
            
        }
        private void ClearAllInputs(Control parent)
        {

            foreach (Control ctrl in parent.Controls) {

                if (ctrl is TextBox tb) {
                    tb.Clear();
                }
                else if (ctrl is ComboBox cb) {
                    cb.SelectedIndex = -1;
                }
                else if (ctrl is CheckBox chk) {

                    chk.Checked = false;
                }
                else if (ctrl is DateTimePicker dtp) {
                    int currentYear = DateTime.Now.Year;
                    dtp.MaxDate = DateTime.Now.AddYears(-15);
                    dtp.MinDate = DateTime.Now.AddYears(-100);
                    dtp.Value = dtp.MaxDate;
                }

                if (ctrl.HasChildren)
                    ClearAllInputs(ctrl);
            }
            lblDiscountedAmount.Text = lblTuitionFee.Text = "P0.00";
            lblStudNumber.Text = studentController.GenerateNewStudentId();  
        }
        private decimal GetDiscount(){
                decimal discount = 0;

                if (rb25.Checked) {
                    discount = .25m; }

                else if (rb50.Checked) {

                    discount = .5m;
                }
                else if (rbFullScholar.Checked) {

                    discount = 1m;
                }
                return discount;
        }
        public void ShowDiscountAmount(){

            decimal discount = GetDiscount();
            decimal tuitionFee = (decimal)cmbProgram.SelectedValue;
            decimal discountedTuition = tuitionFee * discount;

            lblDiscountedAmount.Text = $"P{tuitionFee - discountedTuition:N2}";
        }

        private void cmbYearLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbProgram.SelectedIndex != -1) { 
                decimal tuition = (decimal)cmbProgram.SelectedValue;
                lblTuitionFee.Text = $"₱{tuition:N2}";
            }
        }

        private void rb25_CheckedChanged(object sender, EventArgs e)
        {
            ShowDiscountAmount();
        }

        private void rb50_CheckedChanged(object sender, EventArgs e)
        {
            ShowDiscountAmount();
        }

        private void rbFullScholar_CheckedChanged(object sender, EventArgs e)
        {
            ShowDiscountAmount();
        }
    }
}
