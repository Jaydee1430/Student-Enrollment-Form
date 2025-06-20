using System;
using System.Collections;
using System.Data.SqlClient;

namespace Rosales_UIDesign
{
    // Ang StudentRepository ay ang class na may obligasyon mag transact at mag connect sa database.
    // Dapat nakainherit sa IStudentRepository
    // upang makasigurado na susunod siya sa kusunduan
    //kung ano ang mga dapat gawin (methods) ng StudentRepository
    public class StudentRepository : IStudentRepository
    {
        // _connectionString ito ang mag hahawak ng information kung paano mag connect sa database
        //dapat naka indicate dito kung anong server name at database na gagamitin.
        //at kung paano ang security setup. 
        private readonly string _connectionString;

        public StudentRepository(string connectionString)
        {
            // manggalaling ang value nito kung sino man ang mag instantiate ng StudentRepository
            // sa pamamagitn ng parameter connectionString
            _connectionString = connectionString;
        }

        public ArrayList GetAllStudents()
        {
            // ito ang mag-hold ng lahat ng students galing datbase.
            ArrayList students = new ArrayList();

            //Ito ang code kung paano mag prepare ng pag open ng database.
            //Ginamit ang using keyword para makasigurado na ang database na hindi
            //maiiwan na naka open ang connectiont natin sa database.
            //Sa oras na lumabas ng body ng using, automatic mag close na din ang connection.
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();// dito pa lang mag open ang database

                //ito ang variable na humahawak ng instruction (query) kung ano ang mga kailangan fields, tables at relations ng bawat tables.
                //mapapansin na may naka prefix na stud, guard, prog, req at pay sa bawat columns
                //ang tawag diyan ay "alias", na nag rerepresents ng table na pinanggalingan niya. makitatakita mo ang declaration nito sa 
                //mga inner join clauses.
                string sql = @"SELECT stud.StudentId, stud.FirstName, stud.LastName, stud.MiddleName, stud.Address,
                stud.ContactNumber, stud.DateOfBirth,stud.YearLevel, stud.ProgramId, prog.ProgramName, prog.TuitionFee,
                guard.GuardianId, guard.FirstName, guard.LastName, guard.MiddleName, guard.Address, guard.ContactNumber,
                guard.Relationship, req.RequirementsId, req.BirthCertificate, req.TOR, req.GoodMoral, pay.PaymentId, 
                pay.AmountPaid, pay.Method, pay.ScholarshipDiscount
                FROM dbo.Student stud
                INNER JOIN dbo.Guardian guard ON stud.GuardianId = guard.GuardianId
                INNER JOIN dbo.ProgramInfo prog ON stud.ProgramId = prog.ProgramId
                INNER JOIN dbo.Requirements req ON stud.RequirementsId = req.RequirementsId
                INNER JOIN dbo.Payment pay ON stud.PaymentId = pay.PaymentId";

                //ito naman ang preparation kung paano babasahin at execute ng instruction (query) 
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    //ito naman ang prepration ng reader na siya magbabasa ng loob ng table.
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        // ito naman actual na pag execute ng query
                        // ang while condition ay palaging true hanggat may nababasa pa siya laman (record) ng table.
                        // at false naman kapag wala ng kasunod na record
                        while (reader.Read())
                        {
                            //Prepare Guardian Object
                            //ang reader.GetInt32(11) ay ang code para kunin ang value per column
                            //GetInt32() ang method para iconvert ito sa int, GetString() para iconvert sa string,
                            //ang number sa loog nito tulad 11 sa reader.GetInt32(11) ay ang bilang kung pang ilang
                            //column ang kukunin niyang value.
                            //kung babasahin mo ang laman ng sql variable sa taas
                            //na may value na "SELECT stud.StudentId, stud.FirstName, stud.LastName, ....."
                            //nakasulat doon ang lahat ng columns table na kailangan natin...
                            //ang pagkakasunod ng column ang siya bilang na kelangan natin na nag sisimula sa index 0...
                            // example: kung need natin ang value ng stud.FirstName, ito ay reader.GetString(1),
                            // ang 1 ang ginamit natin kasi siya ay nasa column index 1
                            // dahil ang stud.StudentId ay index 0, stud.FirstName ay index 1, stud.LastName ay index 2
                            // kaya ang reader.GetInt32(11) ay ang value ng guard.GuardianId.
                            Guardian guardian = new Guardian(reader.GetInt32(11),
                                                             reader.GetString(12),
                                                             reader.GetString(14),
                                                             reader.GetString(13),
                                                             reader.GetString(17),
                                                             reader.GetString(15),
                                                             reader.GetString(16));

                            //Prepare Requirements Object
                            Requirements requirements = new Requirements();
                            requirements.Id = reader.GetInt32(18);
                            requirements.BirthCertificate = reader.GetBoolean(19);
                            requirements.TOR = reader.GetBoolean(20);
                            requirements.GoodMoral = reader.GetBoolean(21);

                            //Prepare ProgramInfo Object
                            ProgramInfo programInfo = new ProgramInfo(reader.GetInt32(8), reader.GetString(9),
                                                                      reader.GetDecimal(10));

                            //Prepare Payment Object/
                            Payment payment = new Payment(reader.GetInt32(22), reader.GetDecimal(23), reader.GetString(24), Convert.ToDecimal(reader.GetValue(25)));

                            //Prepare Student Object
                            Student student = new Student(reader.GetString(0), reader.GetString(1), reader.GetString(2),
                                                          reader.GetString(3), reader.GetDateTime(6), programInfo, guardian,
                                                          requirements, payment, reader.GetString(5), reader.GetString(4),
                                                          reader.GetString(7));

                            // iadd na natin ang created nating student sa listahan
                            students.Add(student);
                        }
                    }
                }
            }

            //ipapasa natin ang listahan kung sino man ang tumawag ng methods na ito.
            return students;
        }

        public Student GetStudentById(string id)
        {
            //Ihanda ang object na gagamiting sa pagpasa ng student information
            Student student = null;

            //Basahin ang paliwanag sa loob ng method na GetAllStudents()
            string sql = @"SELECT stud.StudentId, stud.FirstName, stud.LastName, stud.MiddleName, stud.Address,
                stud.ContactNumber, stud.DateOfBirth,stud.YearLevel, stud.ProgramId, prog.ProgramName, prog.TuitionFee,
                guard.GuardianId, guard.FirstName, guard.LastName, guard.MiddleName, guard.Address, guard.ContactNumber,
                guard.Relationship, req.RequirementsId, req.BirthCertificate, req.TOR, req.GoodMoral, pay.PaymentId, 
                pay.AmountPaid, pay.Method, pay.ScholarshipDiscount
                FROM dbo.Student stud
                INNER JOIN dbo.Guardian guard ON stud.GuardianId = guard.GuardianId
                INNER JOIN dbo.ProgramInfo prog ON stud.ProgramId = prog.ProgramId
                INNER JOIN dbo.Requirements req ON stud.RequirementsId = req.RequirementsId
                INNER JOIN dbo.Payment pay ON stud.PaymentId = pay.PaymentId
            WHERE stud.StudentId = @StudentId";

            //Basahin ang paliwanag sa loob ng method na GetAllStudents()
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();//Basahin ang paliwanag sa loob ng method na GetAllStudents()

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    //ito naman ang paglalagay ng parameter sa sql script.
                    //mapapansin sa query sa taas na may WHERE clause.
                    // WHERE stud.StudentId = @StudentId
                    //Ito ay upang ang makuha lang natin record ay ang student na kapareho ng id value natinangap ng method 
                    //na ito. Subalit ang sql query at c# ay magkaibang language at itong code na ito ang nagsisilbing
                    //bridge para maipasa ang variable ng c# papuntang variable ng sql.
                    //@StudentId ang variable nito naman ng sql. ang "@" symbol sa sql ang nagrerepresent ng variable ito.
                    // code na ito ipinapase mo ang laman ng c# varaible ng "id" papuntang sql variable "@StudentId"
                    cmd.Parameters.AddWithValue("@StudentId", id);

                    //Basahin ang paliwanag sa loob ng method na GetAllStudents()
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        //Dahil ang expected natin makukuha ay isa lang dahil wala dapat student na may kaparehong student id.
                        // hindi na natin kailangan sa idaan sa while looping, need na lang natin alamin kung may nabasa ba siya record
                        // or wala.
                        if (reader.Read())
                        {
                            //Prepare Guardian Object
                            //Basahin ang paliwanag sa loob ng method na GetAllStudents()
                            Guardian guardian = new Guardian(reader.GetInt32(11),
                                                             reader.GetString(12),
                                                             reader.GetString(14),
                                                             reader.GetString(13),
                                                             reader.GetString(17),
                                                             reader.GetString(15),
                                                             reader.GetString(16));

                            //Prepare Requirements Object
                            Requirements requirements = new Requirements();
                            requirements.Id = reader.GetInt32(18);
                            requirements.BirthCertificate = reader.GetBoolean(19);
                            requirements.TOR = reader.GetBoolean(20);
                            requirements.GoodMoral = reader.GetBoolean(21);

                            //Prepare ProgramInfo Object
                            ProgramInfo programInfo = new ProgramInfo(reader.GetInt32(8), reader.GetString(9),
                                                                      reader.GetDecimal(10));

                            //Prepare Payment Object/
                            Payment payment = new Payment(reader.GetInt32(22), reader.GetDecimal(23), reader.GetString(24), Convert.ToDecimal(reader.GetValue(25)));

                            //Prepare Student Object
                            student = new Student(reader.GetString(0), reader.GetString(1), reader.GetString(2),
                                                         reader.GetString(3), reader.GetDateTime(6), programInfo, guardian,
                                                         requirements, payment, reader.GetString(5), reader.GetString(4),
                                                         reader.GetString(7));
                        }
                    }
                }
            }
            // ipapasa na natin ang student na nkuha natin sa database sa kung sino man ang tumawang ng method na ito.
            return student;
        }

        public void SaveStudentRegistration(Student student)
        {
            //Basahin ang paliwanag sa loob ng method na GetAllStudents()
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                //Basahin ang paliwanag sa loob ng method na GetAllStudents()
                conn.Open();

                //Dahil maraming insert into na kailagan gawin, tulad nang
                //insert ng record sa guardian, requirements, payment at student tables.
                //kailangna natin ng paraan kung papaano irevert o irollback ang pag insert sa lahat ng tables
                //kapag may isang nag failed.
                //Example: nag success ang pag insert sa guardian, requirements at payment pero nag failed
                // pag insert sa student table, dapat irollback or (hindi ituloy)
                // natin ang pag insert sa lahat ng tables.
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    // Insert Guardian
                    int guardianId = InsertGuardian(conn, transaction, student.Guardian);

                    // Insert Requirements
                    int requirementsId = InsertRequirements(conn, transaction, student.Requirements);

                    // Insert Payment
                    int paymentId = InsertPayment(conn, transaction, student.Payment);

                    // Insert Student
                    InsertStudent(conn, transaction, student, guardianId, requirementsId, paymentId);

                    // Insert Student Registration
                    InsertStudentRegistration(conn, transaction, student.StudentId, requirementsId);

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    // sa oras may error na nangyari sa pag insert sa lahat ng table.
                    // irorollback ang lahat ng inserts
                    transaction.Rollback();

                    // ipapasa ang customized exception para matanggap ng tumawag ang error message
                    throw new Exception("Error saving student registration: " + ex.Message);
                }
            }
        }

        public void DeleteStudent(string id)
        {
            try
            {
                //Basahin ang paliwanag sa loob ng method na GetAllStudents()
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    //Basahin ang paliwanag sa loob ng method na GetAllStudents()
                    conn.Open();

                    string deleteRegistration = "DELETE FROM dbo.StudentRegistration WHERE StudentId = @Id";
                    using (SqlCommand regCmd = new SqlCommand(deleteRegistration, conn))
                    {
                        regCmd.Parameters.AddWithValue("@Id", id);
                        regCmd.ExecuteNonQuery();
                    }

                    string deleteStudent = "DELETE FROM dbo.Student WHERE StudentId = @Id";
                    //Basahin ang paliwanag sa loob ng method na GetAllStudents()
                    using (SqlCommand studentCmd = new SqlCommand(deleteStudent, conn))
                    {
                        //Basahin ang paliwanag sa loob ng method na GetAllStudents()
                        studentCmd.Parameters.AddWithValue("@Id", id);

                        //ExecuteNonQuery() naman ang metod na tinatawag kapag may kailangan ireturn na record.
                        //rowsAffected ang makakatanggap kung ilang ang record na affected.
                        int rowsAffected = studentCmd.ExecuteNonQuery();
                        if (rowsAffected == 0)
                        {
                            Console.WriteLine("No student found with that ID.");
                        }
                        else
                        {
                            Console.WriteLine("Student deleted successfully.");
                        }
                    }


                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting student registration: " + ex.Message);
            }
        }

        public ArrayList GetAllPrograms()
        {
            //Basahin ang paliwanag sa loob ng method na GetAllStudents()
            ArrayList programList = new ArrayList();

            string sql = "SELECT ProgramId, ProgramName, TuitionFee FROM ProgramInfo";
            //Basahin ang paliwanag sa loob ng method na GetAllStudents()
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                //Basahin ang paliwanag sa loob ng method na GetAllStudents()
                conn.Open();
                //Basahin ang paliwanag sa loob ng method na GetAllStudents()
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    //Basahin ang paliwanag sa loob ng method na GetAllStudents()
                    while (reader.Read())
                    {
                        ProgramInfo program = new ProgramInfo
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("ProgramId")),
                            ProgramName = reader.GetString(reader.GetOrdinal("ProgramName")),
                            TuitionFee = reader.GetDecimal(reader.GetOrdinal("TuitionFee"))
                        };
                        //Basahin ang paliwanag sa loob ng method na GetAllStudents()
                        programList.Add(program);
                    }
                }
            }
            //Basahin ang paliwanag sa loob ng method na GetAllStudents()
            return programList;
        }

        public string GenerateNewStudentId()
        {
            //ito yung first3 digits ng student it as school code
            // 084-2025-0001
            string prefix = "084";
            //ito naman yung 2nd part ng id
            string year = DateTime.Now.Year.ToString();
            //ito nag interval ng id
            int nextSequence = 1;

            string sql = @"
                SELECT TOP 1 StudentId
                FROM dbo.Student
                WHERE StudentId LIKE @YearFilter
                ORDER BY StudentId DESC";
            //Basahin ang paliwanag sa loob ng method na GetAllStudents()
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                //Basahin ang paliwanag sa loob ng method na GetAllStudents()
                conn.Open();
                //Basahin ang paliwanag sa loob ng method na GetAllStudents()
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    // ito yung pag pasa value papuntang sql variable na "@YearFilter"
                    // para maFilter by current year, e.g. "%-2025-%"
                    // ibig sabhin kukunin latest record ang year sa id ay equals sa current year.
                    cmd.Parameters.AddWithValue("@YearFilter", $"%-{year}-%");

                    //ExecuteScalar() naman ang metod na tinatawag kapag may kailangan ng single value
                    //or hindi isang buong record ang kailangan ireturn.
                    //at base sa script sa taas, "SELECT TOP 1 StudentId..."
                    //StudentId lang ang irereturn niya
                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        //iconvert niya sa string ang nireturn galing database.
                        // e.g. "084-2025-0032"
                        string latestId = result.ToString();
                        //kukunin niya nag last 4 digits sa id. eg: 0032
                        string lastFour = latestId.Substring(latestId.Length - 4);
                        //i-try niya i-parse
                        if (int.TryParse(lastFour, out int numericPart))
                        {
                            // at kung puwede iconvert sa number, i-add niya ng 1.
                            nextSequence = numericPart + 1; // 33
                        }
                    }
                }
            }

            // Compose new ID: "084-2025-0033"
            return $"{prefix}-{year}-{nextSequence.ToString("D4")}";
        }

        private int InsertGuardian(SqlConnection conn, SqlTransaction tran, Guardian guardian)
        {
            string query = @"INSERT INTO Guardian (FirstName, LastName, MiddleName, Address, ContactNumber, Relationship)
                         OUTPUT INSERTED.GuardianId
                         VALUES (@FirstName, @LastName, @MiddleName, @Address, @Contact, @Relationship)";
            //Basahin ang paliwanag sa loob ng method na GetAllStudents()
            using (SqlCommand cmd = new SqlCommand(query, conn, tran))
            {
                SetupGuardianParameters(cmd, guardian);
                //Basahin ang paliwanag sa loob ng method na GenerateNewStudentId
                return (int)cmd.ExecuteScalar();
            }
        }

        private int InsertRequirements(SqlConnection conn, SqlTransaction tran, Requirements req)
        {
            string query = @"INSERT INTO Requirements (BirthCertificate, TOR, GoodMoral)
                         OUTPUT INSERTED.RequirementsId
                         VALUES (@BC, @TOR, @GM)";

            //Basahin ang paliwanag sa loob ng method na GetAllStudents()
            using (SqlCommand cmd = new SqlCommand(query, conn, tran))
            {
                SetupRequirementsParameters(cmd, req);
                //Basahin ang paliwanag sa loob ng method na GenerateNewStudentId()
                return (int)cmd.ExecuteScalar();
            }
        }

        private int InsertPayment(SqlConnection conn, SqlTransaction tran, Payment pay)
        {
            string query = @"INSERT INTO Payment (AmountPaid,Method,ScholarshipDiscount)
                         OUTPUT INSERTED.PaymentId
                         VALUES (@Amount,@Method,@ScholarshipDiscount)";

            //Basahin ang paliwanag sa loob ng method na GetAllStudents()
            using (SqlCommand cmd = new SqlCommand(query, conn, tran))
            {
                SetupPaymentParameters(cmd, pay);
                //Basahin ang paliwanag sa loob ng method na GenerateNewStudentId()
                return (int)cmd.ExecuteScalar();
            }
        }

        private void InsertStudent(SqlConnection conn, SqlTransaction tran, Student student, int guardianId, int requirementsId, int paymentId)
        {
            string query = @"INSERT INTO Student (StudentId, FirstName, LastName, MiddleName, Address, ContactNumber, DateOfBirth, ProgramId, 
                                GuardianId, RequirementsId, PaymentId, YearLevel)
                         VALUES (@StudentId, @FirstName, @LastName, @MiddleName, @Address, @Contact, @DOB, @ProgramId, @GuardianId, @RequirementsId, 
                                @PaymentId, @YearLevel)";

            //Basahin ang paliwanag sa loob ng method na GetAllStudents()
            using (SqlCommand cmd = new SqlCommand(query, conn, tran))
            {
                SetupStudentParameters(conn, tran, cmd, student, guardianId, requirementsId, paymentId);
                //Basahin ang paliwanag sa loob ng method na DeleteStudent()
                cmd.ExecuteNonQuery();
            }
        }

        private void InsertStudentRegistration(SqlConnection conn, SqlTransaction tran, string studentId, int requirementsId)
        {
            string query = @"INSERT INTO StudentRegistration (StudentId, RequirementsId, IsValid)
                         VALUES (@StudentId, @RequirementsId, @IsValid)";

            using (SqlCommand cmd = new SqlCommand(query, conn, tran))
            {
                //ito naman ang paglalagay ng parameter sa sql script.
                //mapapansin sa query sa taas na may VALUES clause.
                // VALUES (@StudentId, @RequirementsId, @IsValid)
                //Ang sql query at c# ay magkaibang language at itong code na ito ang nagsisilbing
                //bridge para maipasa ang variable ng c# papuntang variable ng sql.
                //@StudentId ang variable nito naman ng sql. ang "@" symbol sa sql ang nagrerepresent ng variable ito.
                // code na ito ipinapase mo ang laman ng c# varaible ng "id" papuntang sql variable "@StudentId"
                cmd.Parameters.AddWithValue("@StudentId", studentId);
                cmd.Parameters.AddWithValue("@RequirementsId", requirementsId);
                cmd.Parameters.AddWithValue("@IsValid", true); // Based on Requirements.AllSubmitted()

                //Basahin ang paliwanag sa loob ng method na DeleteStudent()
                cmd.ExecuteNonQuery();
            }
        }

        private int GetProgramId(SqlConnection conn, SqlTransaction tran, string programName)
        {
            string query = "SELECT ProgramId FROM ProgramInfo WHERE ProgramName = @Name";
            using (SqlCommand cmd = new SqlCommand(query, conn, tran))
            {
                cmd.Parameters.AddWithValue("@Name", programName);
                object result = cmd.ExecuteScalar();
                return result != null ? Convert.ToInt32(result) : throw new Exception("Program not found.");
            }
        }

        public void UpdateStudentRegistration(Student student)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                //Basahin ang paliwanag sa loob ng method na GetAllStudents()
                conn.Open();

                //Dahil maraming insert into na kailagan gawin, tulad nang
                //update ng record sa guardian, requirements, payment at student tables.
                //kailangna natin ng paraan kung papaano irevert o irollback ang pag insert sa lahat ng tables
                //kapag may isang nag failed.
                //Example: nag success ang pag insert sa guardian, requirements at payment pero nag failed
                // pag update sa student table, dapat irollback or (hindi ituloy)
                // natin ang pag update sa lahat ng tables.
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    // Update Guardian
                    UpdateGuardian(conn, transaction, student.Guardian);

                    // Update Requirements
                    UpdateRequirements(conn, transaction, student.Requirements);

                    // Update Payment
                    UpdatePayment(conn, transaction, student.Payment);

                    // Update Student
                    UpdateStudent(conn, transaction, student);

                    

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception("Error updating student registration: " + ex.Message);
                }
            }
        }

        private void UpdateGuardian(SqlConnection conn, SqlTransaction transaction, Guardian guardian)
        {
            string sql = @"UPDATE Guardian
                   SET FirstName = @FirstName,
                       LastName = @LastName,
                       MiddleName = @MiddleName,
                       Address = @Address,
                       ContactNumber = @Contact,
                       Relationship = @Relationship
                   WHERE GuardianId = @GuardianId";

            using (SqlCommand cmd = new SqlCommand(sql, conn, transaction))
            {
                SetupGuardianParameters(cmd, guardian);
                cmd.Parameters.AddWithValue("@GuardianId", guardian.Id);

                //Basahin ang paliwanag sa loob ng method na DeleteStudent()
                cmd.ExecuteNonQuery();
            }
        }

        private void UpdateRequirements(SqlConnection conn, SqlTransaction transaction, Requirements requirements)
        {
            string sql = @"UPDATE Requirements
                   SET BirthCertificate = @BC,
                       TOR = @TOR,
                       GoodMoral = @GM
                   WHERE RequirementsId = @RequirementsId";

            using (SqlCommand cmd = new SqlCommand(sql, conn, transaction))
            {
                SetupRequirementsParameters(cmd, requirements);
                cmd.Parameters.AddWithValue("@RequirementsId", requirements.Id);

                //Basahin ang paliwanag sa loob ng method na DeleteStudent()
                cmd.ExecuteNonQuery();
            }
        }

        private void UpdatePayment(SqlConnection conn, SqlTransaction transaction, Payment payment)
        {
            string sql = @"UPDATE Payment
                   SET AmountPaid = @Amount,
                       Method = @Method,
                       ScholarshipDiscount = @ScholarshipDiscount
                   WHERE PaymentId = @PaymentId";

            using (SqlCommand cmd = new SqlCommand(sql, conn, transaction))
            {
                SetupPaymentParameters(cmd, payment);
                cmd.Parameters.AddWithValue("@PaymentId", payment.Id);

                //Basahin ang paliwanag sa loob ng method na DeleteStudent()
                cmd.ExecuteNonQuery();
            }
        }

        private void UpdateStudent(SqlConnection conn, SqlTransaction transaction, Student student)
        {
            string sql = @"UPDATE Student
                   SET FirstName = @FirstName,
                       LastName = @LastName,
                       MiddleName = @MiddleName,
                       Address = @Address,
                       ContactNumber = @Contact,
                       DateOfBirth = @DOB,
                       ProgramId = @ProgramId,
                       GuardianId = @GuardianId,
                       RequirementsId = @RequirementsId,
                       PaymentId = @PaymentId,
                       YearLevel = @YearLevel
                   WHERE StudentId = @StudentId";

            using (SqlCommand cmd = new SqlCommand(sql, conn, transaction))
            {
                SetupStudentParameters(conn, transaction, cmd, student, student.Guardian.Id, student.Requirements.Id, student.Payment.Id);
                //Basahin ang paliwanag sa loob ng method na DeleteStudent()
                cmd.ExecuteNonQuery();
            }
        }

        private void SetupGuardianParameters(SqlCommand cmd, Guardian guardian)
        {
            cmd.Parameters.AddWithValue("@FirstName", guardian.FirstName);
            cmd.Parameters.AddWithValue("@LastName", guardian.LastName);
            cmd.Parameters.AddWithValue("@MiddleName", guardian.MiddleName ?? "");
            cmd.Parameters.AddWithValue("@Address", guardian.Address);
            cmd.Parameters.AddWithValue("@Contact", guardian.ContactNumber);
            cmd.Parameters.AddWithValue("@Relationship", guardian.Relationship);
        }

        private void SetupRequirementsParameters(SqlCommand cmd, Requirements req)
        {
            cmd.Parameters.AddWithValue("@BC", req.BirthCertificate);
            cmd.Parameters.AddWithValue("@TOR", req.TOR);
            cmd.Parameters.AddWithValue("@GM", req.GoodMoral);
        }

        private void SetupPaymentParameters(SqlCommand cmd, Payment pay)
        {
            cmd.Parameters.AddWithValue("@Amount", pay.AmountPaid);
            cmd.Parameters.AddWithValue("@Method", pay.Method);
            cmd.Parameters.AddWithValue("@ScholarshipDiscount", pay.ScholarshipDiscount);
        }

        private void SetupStudentParameters(SqlConnection conn, SqlTransaction tran, SqlCommand cmd, Student student, int guardianId, int requirementsId, int paymentId)
        {
            cmd.Parameters.AddWithValue("@StudentId", student.StudentId);
            cmd.Parameters.AddWithValue("@FirstName", student.FirstName);
            cmd.Parameters.AddWithValue("@LastName", student.LastName);
            cmd.Parameters.AddWithValue("@MiddleName", student.MiddleName ?? "");
            cmd.Parameters.AddWithValue("@Address", student.Address);
            cmd.Parameters.AddWithValue("@Contact", student.ContactNumber);
            cmd.Parameters.AddWithValue("@DOB", student.DateOfBirth);
            cmd.Parameters.AddWithValue("@ProgramId", GetProgramId(conn, tran, student.Program.ProgramName));
            cmd.Parameters.AddWithValue("@GuardianId", guardianId);
            cmd.Parameters.AddWithValue("@RequirementsId", requirementsId);
            cmd.Parameters.AddWithValue("@PaymentId", paymentId);
            cmd.Parameters.AddWithValue("@YearLevel", student.YearLevel);
        }

    }
}

