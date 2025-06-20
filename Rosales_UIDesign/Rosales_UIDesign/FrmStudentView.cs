
using System;
using System.Collections;
using System.Windows.Forms;
using static System.Net.WebRequestMethods;

namespace Rosales_UIDesign
{
    public partial class FrmStudentView : Form
    {
        ArrayList studentList = new ArrayList();
        private string connectionString = "Data Source=JAYDEE\\SQLEXPRESS;Initial Catalog=StudentRegDB;Integrated Security=True;" + "TrustServerCertificate=True";
        StudentController studentController;
        public FrmStudentView()
        {
            InitializeComponent();  
            StudentRepository repo = new StudentRepository(connectionString);
            studentController = new StudentController(repo);
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                // kunin ang selected row sa datagrid view.

                DataGridViewRow row = dgridStudentList.Rows[e.RowIndex];

                // Read values by column name or index

                string studentId = row.Cells["StudentId"].Value?.ToString();

                //kunin ang record sa database gamit nag selected student id

                Student selectedStudent = studentController.GetStudentById(studentId);

                // Isa isahin ang labels para madisplay ang lahat ng values

                lblStudNumber.Text = selectedStudent.StudentId.ToString();

                lblStudentName.Text = $"{selectedStudent.LastName}, {selectedStudent.FirstName} {selectedStudent.MiddleName}";
                lblDOB.Text = selectedStudent.DateOfBirth.ToShortDateString();

                lblStudentAddress.Text = selectedStudent.Address;

                lblContact.Text = selectedStudent.ContactNumber;

                lblYearLevel.Text = selectedStudent.YearLevel;

                Guardian guardian = selectedStudent.Guardian;

                lblGName.Text = $"{guardian.LastName}, {guardian.FirstName} {guardian.MiddleName}";
                lblGAddress.Text = guardian.Address;

                lblGContact.Text = guardian.ContactNumber;

                lblGRelationship.Text = guardian.Relationship;

                ProgramInfo programInfo = selectedStudent.Program;

                lblProgram.Text = programInfo.ProgramName;

                lblTuition.Text = $"P {programInfo.TuitionFee:N2}";

                Requirements requirements = selectedStudent.Requirements;

                lblBirth.Text = requirements.BirthCertificate ? "✔ Birth Certificate" : "Birth Certificate";
                lblTOR.Text = requirements.TOR ? "✔ Transcript of Record" : "Transcript of Record";
                lblGM.Text = requirements.GoodMoral ? "✔ Good Morale" : "Good Morale";

                Payment payment = selectedStudent.Payment;

                lblDiscount.Text = $"{payment.ScholarshipDiscount * 100} %";

                lblDiscountedFee.Text = $"P {programInfo.TuitionFee * payment.ScholarshipDiscount:N2}";
                lblPay.Text = $"P {payment.AmountPaid:N2}";

                lblMethod.Text = payment.Method;
            }
        }
        private void SetupDataGrid()
        {
            //Create a List of Student

            //tawagin ang method na kukuha ng list galing database]

            studentList = studentController.GetAllStudents();

            // Bind to datagrid

            dgridStudentList.DataSource = studentList;

            // huwag ipapakita ang value ng program, guardian requirements

            // at payment sa datagrid view.

            if (dgridStudentList.Columns.Contains("Program"))
            {
                dgridStudentList.Columns["Program"].Visible = false;
            }
            if (dgridStudentList.Columns.Contains("Guardian"))
            {
                dgridStudentList.Columns["Guardian"].Visible = false;
            }
            if (dgridStudentList.Columns.Contains("Requirements"))
            {
                dgridStudentList.Columns["Requirements"].Visible = false;
            }
            if (dgridStudentList.Columns.Contains("Payment"))
            {
                dgridStudentList.Columns["Payment"].Visible = false;
            }

            // autmotic expand ang datagrid depende sa size ng form.

            dgridStudentList.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            //kung sakali may lang atleast isa

            if (dgridStudentList.Rows.Count > 0 && dgridStudentList.Columns.Count > 0)
            {

                {
                    //tawagin maually ang cellclick para madisplay ang values sa bawat labels sa upper panel ng form
                    DataGridViewCellEventArgs args = new DataGridViewCellEventArgs(0, 0);
                    dataGridView1_CellClick(dgridStudentList, args);

                }
            }
        }

        private void FrmStudentView_Load(object sender, EventArgs e)
        {
            SetupDataGrid();
            HideUpdateInformation();
            btnSubmit.Visible = false;
            int currentYear = DateTime.Now.Year;
            date.MaxDate = DateTime.Now.AddYears(-15);
            date.MinDate = DateTime.Now.AddYears(-100);
            date.Value = date.MaxDate;
        }

        private void FrmStudentView_Activated(object sender, EventArgs e)
        {
            SetupDataGrid();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgridStudentList.CurrentRow != null)
            {
                string studentId = dgridStudentList.CurrentRow.Cells["StudentId"].Value.ToString();

                DialogResult result = MessageBox.Show("Do you want to delete this student?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    studentController.DeleteStudent(studentId);

                    MessageBox.Show("Student deleted successfully.");

                    SetupDataGrid();
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            btnSubmit.Visible = true;
            btnUpdate.Visible = false;

            HideLbl();
            ShowUpdateInformation();

            string[] name = lblStudentName.Text.Split(' ');
            string[] gname = lblGName.Text.Split(' ');
            addressBox.Text = lblStudentAddress.Text;
            firstNameBox.Text = name[1];
            lastNameBox.Text = name[0];
            middleNameBox.Text = name[2];
            contactBox.Text = lblContact.Text;
            glastBox.Text = gname[0];
            gFirstBox.Text = gname[1];
            gMiddleBox.Text = gname[2];
            gAdressBox.Text = lblGAddress.Text;
            gContactBox.Text = lblGContact.Text;
            relationshipBox.Text = lblGRelationship.Text;

        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (dgridStudentList.CurrentRow == null)
            {
                MessageBox.Show("No student selected.");
                return;
            }

            string studentId = dgridStudentList.CurrentRow.Cells["StudentId"].Value.ToString();

            Student studentToUpdate = studentController.GetStudentById(studentId);

            studentToUpdate.FirstName = firstNameBox.Text;
            studentToUpdate.LastName = lastNameBox.Text;
            studentToUpdate.MiddleName = middleNameBox.Text;
            studentToUpdate.Address = addressBox.Text;
            studentToUpdate.ContactNumber = contactBox.Text;

            studentToUpdate.Guardian.FirstName = gFirstBox.Text;
            studentToUpdate.Guardian.LastName = glastBox.Text;
            studentToUpdate.Guardian.MiddleName = gMiddleBox.Text;
            studentToUpdate.Guardian.Address = gAdressBox.Text;
            studentToUpdate.Guardian.ContactNumber = gContactBox.Text;
            studentToUpdate.Guardian.Relationship = relationshipBox.Text;

            studentToUpdate.Requirements.BirthCertificate = chkbirth.Checked;
            studentToUpdate.Requirements.GoodMoral = chkGM.Checked;
            studentToUpdate.Requirements.TOR = chktor.Checked;


            
            studentToUpdate.DateOfBirth = date.Value;

            try
            {
                studentController.UpdateStudentRegistration(studentToUpdate);
                MessageBox.Show("Student information updated successfully!");

                btnSubmit.Visible = false;
                btnUpdate.Visible = true;

                HideUpdateInformation();
                ShowLbl();

                SetupDataGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating student: " + ex.Message);
            }
        }
        private void ShowUpdateInformation()
        {
            addressBox.Visible = true;
            firstNameBox.Visible = true;
            lastNameBox.Visible = true;
            middleNameBox.Visible = true;
            contactBox.Visible = true;
            glastBox.Visible = true;
            gFirstBox.Visible = true;
            gMiddleBox.Visible = true;
            gAdressBox.Visible = true;
            gContactBox.Visible = true;
            relationshipBox.Visible = true;
            chkbirth.Visible = true;
            chkGM.Visible = true;
            chktor.Visible = true;
            date.Visible = true;
        }
        private void HideUpdateInformation()
        {
            addressBox.Visible = false;
            firstNameBox.Visible = false;
            lastNameBox.Visible = false;
            middleNameBox.Visible = false;
            contactBox.Visible = false;
            glastBox.Visible = false;
            gFirstBox.Visible = false;
            gMiddleBox.Visible = false;
            gAdressBox.Visible = false;
            gContactBox.Visible = false;
            gContactBox.Visible = false;
            relationshipBox.Visible = false;
            chkbirth.Visible = false;
            chkGM.Visible = false;
            chktor.Visible= false;
            date.Visible = false;
        }
        private void ShowLbl()
        {
            lblStudentAddress.Visible = true;
            lblStudentName.Visible = true;
            lblContact.Visible = true;
            lblGAddress.Visible = true;
            lblGContact.Visible = true;
            lblGName.Visible = true;
            lblGRelationship.Visible = true;
            btnUpdate.Visible = true;
            lblBirth.Visible = true;
            lblGM.Visible = true;
            lblTOR.Visible = true;
            lblDOB.Visible = true;
        }
        private void HideLbl()
        {
            lblStudentAddress.Visible = false;
            lblStudentName.Visible = false;
            lblContact.Visible = false;
            lblGAddress.Visible = false;
            lblGContact.Visible = false;
            lblGName.Visible = false;
            lblGRelationship.Visible = false;
            btnUpdate.Visible = false;
            lblBirth.Visible = false;
            lblGM.Visible = false;
            lblTOR.Visible = false;
            lblDOB.Visible = false;
        }
    }
}
