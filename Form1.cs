using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Globalization;

namespace UPharmacy
{
    public partial class Form1 : Form
    {
        private TabControl tabControl;
        private TabPage tabPMR, tabDB;
        private DataGridView dailyPatientList,
                             pastPrescriptionList,
                             Patient,
                             currentPrescriptionDetails,
                             previousPrescriptionDetails;
        private TextBox txtContent, txtDay;
        private ListView listViewMemo;
        private Button btnInsert, btnDelete, btnQR;


        private HashSet<string> seenPatientsForDailyList = new HashSet<string>();
        private HashSet<string> seenPatientsForInfo = new HashSet<string>();
        private int dailyNo = 1;
        private int dailyPatientCount = 0;  // 전역에서 누적 count 유지

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = "UPharmacy";
            this.Width = 1200;
            this.Height = 800;
            this.StartPosition = FormStartPosition.CenterScreen;
            Color unifiedBackColor = Color.FromArgb(180, 219, 200);
            this.BackColor = unifiedBackColor;

            tabControl = new TabControl { Dock = DockStyle.Fill };
            tabPMR = new TabPage("PMR");
            tabControl.TabPages.Add(tabPMR);
            this.Controls.Add(tabControl);

            // DBTab 페이지를 생성하여 TabControl에 추가
            DBTab dbTab = new DBTab();
            tabControl.TabPages.Add(dbTab);
            this.Controls.Add(tabControl);

            // 스타일
            ApplyBackColorToAllControls(this, unifiedBackColor);

            RoundedGroupBox groupDailyPatients = new RoundedGroupBox
            {
                Text = "일일환자 리스트",
                Location = new Point(20, 20),
                Size = new Size(520, 230),
                BackColor = Color.White,
                ForeColor = Color.Black,
                Font = new Font("맑은 고딕", 9, FontStyle.Bold),
                CornerRadius = 20
            };
            tabPMR.Controls.Add(groupDailyPatients);

            // DataGridView 생성
            dailyPatientList = new DataGridView
            {
                Location = new Point(20, 20),
                Size = new Size(480, 200),
                BackColor = Color.White,
            };

            dailyPatientList.Columns.Add("dailyPatientDate", "일자");
            dailyPatientList.Columns.Add("dailyPatientNo", "NO");
            //dailyPatientList.Columns.Add("dailyType", "종별");
            dailyPatientList.Columns.Add("daliyPatientName", "고객명");
            dailyPatientList.Columns.Add("dailyResident", "주민번호");
            dailyPatientList.Columns.Add("dailyDoctorName", "의사명");
            dailyPatientList.Columns.Add("dailyDays", "일수");
            dailyPatientList.Columns.Add("dailyPaymentAmount", "영수액"); 
            groupDailyPatients.Controls.Add(dailyPatientList);
            StyleGrid(dailyPatientList); //스타일

            // 과거조제 리스트 GroupBox 생성
            RoundedGroupBox groupPastPrescriptions = new RoundedGroupBox
            {
                Text = "과거조제 리스트",
                Location = new Point(20, 260),
                Size = new Size(520, 230), // DataGridView보다 조금 크게!
                BackColor = Color.White,
                ForeColor = Color.Black,
                Font = new Font("맑은 고딕", 9, FontStyle.Bold),
                CornerRadius = 20
            };
            tabPMR.Controls.Add(groupPastPrescriptions);
            //과거조제 리스트
            pastPrescriptionList = new DataGridView
            {
                Location = new Point(20, 20), // 아래 여백 포함
                Size = new Size(480, 200),
                Anchor = AnchorStyles.Top | AnchorStyles.Left
            };
            pastPrescriptionList.Columns.Add("pastPrescriptionNo", "NO");
            pastPrescriptionList.Columns.Add("pastDate", "날짜");
            pastPrescriptionList.Columns.Add("pastHospitalName", "병의원명");
            pastPrescriptionList.Columns.Add("pastDoctorName", "의사명");
           
            pastPrescriptionList.Columns.Add("pastPayment", "영수액");
            groupPastPrescriptions.Controls.Add(pastPrescriptionList);
            StyleGrid(pastPrescriptionList); //스타일

            //고객메모 
            RoundedGroupBox groupMemo = new RoundedGroupBox
            {
                Text = "고객 메모",
                Location = new Point(20, 500),
                Size = new Size(520, 180),
                BackColor = Color.White,
                ForeColor = Color.Black,
                Font = new Font("맑은 고딕", 9, FontStyle.Bold),
                CornerRadius = 20
            };
            tabPMR.Controls.Add(groupMemo);

            // ListView 생성
            listViewMemo = new ListView
            {
                Location = new Point(20, 30),
                Size = new Size(480, 100),
                View = View.Details,
                FullRowSelect = true,
                GridLines = true
            };
            listViewMemo.Columns.Add("메모 내용", 300);
            listViewMemo.Columns.Add("일자", 150);
            groupMemo.Controls.Add(listViewMemo);


            // 메모 입력용 TextBox
            txtContent = new TextBox
            {
                Location = new Point(20, 135),
                Size = new Size(220, 25)
            };
            groupMemo.Controls.Add(txtContent);

            // 날짜 입력용 TextBox
            txtDay = new TextBox
            {
                Location = new Point(250, 135),
                Size = new Size(100, 25)
            };
            groupMemo.Controls.Add(txtDay);

            // 추가 버튼
            btnInsert = new Button
            {
                Text = "추가",
                Location = new Point(370, 135),
                Size = new Size(60, 25)
            };
            groupMemo.Controls.Add(btnInsert);
            StyleButton(btnInsert);
            btnInsert.Click += btnInsert_Click;

            // 삭제 버튼 (필드 변수 사용)
            btnDelete = new Button
            {
                Text = "삭제",
                Location = new Point(440, 135),
                Size = new Size(60, 25)
            };
            groupMemo.Controls.Add(btnDelete);
            StyleButton(btnDelete);
            btnDelete.Click += btnDelete_Click;

            // 환자정보 GroupBox
            RoundedGroupBox groupPatient = new RoundedGroupBox
            {
                Text = "환자정보",
                Location = new Point(550, 20),
                Size = new Size(600, 120),
                BackColor = Color.White,
                ForeColor = Color.Black,
                Font = new Font("맑은 고딕", 9, FontStyle.Bold),
                CornerRadius = 20
            };
            tabPMR.Controls.Add(groupPatient);

            // 환자정보
            Patient = new DataGridView
            {
                Location = new Point(20, 25),
                Size = new Size(560, 80)
            };
            Patient.Columns.Add("Name", "고객명");
            //Patient.Columns.Add("Type", "종별");
            Patient.Columns.Add("Jumin", "주민번호");
            //Patient.Columns.Add("phone", "연락처");
            //Patient.Columns.Add("bohum", "피보험자");
            Patient.Columns.Add("44(남)", "연령(성별)");
            Patient.Columns.Add("last_Date", "최근조제일");
            Patient.Columns.Add("Hospital", "병의원명");
            Patient.Columns.Add("important", "조제시참고사항");
            groupPatient.Controls.Add(Patient);
            StyleGrid(Patient);

            // 현재조제내역 GroupBox
            RoundedGroupBox groupToday = new RoundedGroupBox
            {
                Text = "현재 조제내역",
                Location = new Point(550, 160),
                Size = new Size(600, 300),
                BackColor = Color.White,
                ForeColor = Color.Black,
                Font = new Font("맑은 고딕", 9, FontStyle.Bold),
                CornerRadius = 20
            };
            tabPMR.Controls.Add(groupToday);
            currentPrescriptionDetails = new DataGridView
            {
                Location = new Point(20, 30),
                Size = new Size(560, 250),
            };
            currentPrescriptionDetails.Columns.Add("currentDrugNo", "NO");
            currentPrescriptionDetails.Columns.Add("currentDrugCode", "약품코드");
            currentPrescriptionDetails.Columns.Add("currentDrugName", "약품명");
            currentPrescriptionDetails.Columns.Add("currentDosePerDay", "1회량");
            currentPrescriptionDetails.Columns.Add("currentTime", "횟수");
            currentPrescriptionDetails.Columns.Add("currentDays", "일수");
            currentPrescriptionDetails.Columns.Add("currentTotal", "총량");
            currentPrescriptionDetails.Columns.Add("currentInsurance", "보험"); //건강보험(1)
            currentPrescriptionDetails.Columns.Add("currentUnitPrice", "단가"); //100으로 통일
            currentPrescriptionDetails.Columns.Add("currentPrice", "금액"); // sum(단가*총량)
            //currentPrescriptionDetails.Columns.Add("currentOrderStatus", "형태");
            groupToday.Controls.Add(currentPrescriptionDetails);
            StyleGrid(currentPrescriptionDetails);

            // 과거조제내역 
            RoundedGroupBox groupPastDetail = new RoundedGroupBox
            {
                Text = "과거 조제내역",
                Location = new Point(550, 480),
                Size = new Size(600, 200),
                BackColor = Color.White,
                ForeColor = Color.Black,
                Font = new Font("맑은 고딕", 9, FontStyle.Bold),
                CornerRadius = 20
            };
            tabPMR.Controls.Add(groupPastDetail);
            previousPrescriptionDetails = new DataGridView
            {
                Location = new Point(20, 30),
                Size = new Size(560, 150),
            };
            previousPrescriptionDetails.Columns.Add("previousDrugNo", "NO");
            previousPrescriptionDetails.Columns.Add("previousdate", "날짜");
            previousPrescriptionDetails.Columns.Add("previousDrug", "약품코드"); 
            previousPrescriptionDetails.Columns.Add("previousDrugName", "약품명");
            previousPrescriptionDetails.Columns.Add("previousDosePerDay", "1회량");
            previousPrescriptionDetails.Columns.Add("previousTime", "횟수");
            previousPrescriptionDetails.Columns.Add("previousDays", "일수");
            previousPrescriptionDetails.Columns.Add("previousTotal", "총량");
            previousPrescriptionDetails.Columns.Add("previousInsurance", "보험"); 
            previousPrescriptionDetails.Columns.Add("previousUnitPrice", "단가");
            previousPrescriptionDetails.Columns.Add("previousPrice", "금액");
            //previousPrescriptionDetails.Columns.Add("previousOrderStatus", "형태");
            groupPastDetail.Controls.Add(previousPrescriptionDetails);
            StyleGrid(previousPrescriptionDetails);

            // csv 불러오기 버튼 
            btnQR = new Button
            {
                Text = "처방전",
                Location = new Point(1080, 690),
                Size = new Size(60, 25)
            };
            tabPMR.Controls.Add(btnQR);
            StyleButton(btnQR);
            btnQR.Click += btnQR_Click;

        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtContent.Text) || string.IsNullOrWhiteSpace(txtDay.Text))
            {
                MessageBox.Show("메모 내용과 일자를 입력해주세요.");
                return;
            }
            string[] str = { txtContent.Text.Trim(), txtDay.Text.Trim() };
            ListViewItem item = new ListViewItem(str);
            listViewMemo.Items.Add(item);

            txtContent.Clear();
            txtDay.Clear();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (listViewMemo.SelectedItems.Count > 0)
            {
                listViewMemo.Items.Remove(listViewMemo.SelectedItems[0]);
            }
            else
            {
                MessageBox.Show("삭제할 메모를 선택하세요.");
            }
        }
   
        //스타일
        private void ApplyBackColorToAllControls(Control parent, Color backColor)
        {
            foreach (Control control in parent.Controls)
            {
                control.BackColor = backColor;

                if (control is GroupBox || control is Label)
                    control.ForeColor = Color.Black;

                if (control.HasChildren)
                {
                    ApplyBackColorToAllControls(control, backColor);
                }
            }
        }
        // 스타일 - DataGridView
        private void StyleGrid(DataGridView grid)
        {
            grid.BackgroundColor = Color.White;
            grid.BorderStyle = BorderStyle.FixedSingle;
            grid.EnableHeadersVisualStyles = false;
            grid.ColumnHeadersDefaultCellStyle.BackColor = Color.LightGray;
            grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            grid.ColumnHeadersDefaultCellStyle.Font = new Font("맑은 고딕", 9, FontStyle.Bold);
            grid.DefaultCellStyle.SelectionBackColor = Color.LightSkyBlue;
            grid.DefaultCellStyle.Font = new Font("맑은 고딕", 9);
            grid.GridColor = Color.LightGray;
            grid.RowHeadersVisible = false;
            grid.AllowUserToAddRows = false;
            grid.AllowUserToDeleteRows = false;
            grid.ReadOnly = true;
        }

        //스타일 - 버튼
        private void StyleButton(Button btn)
        {
            btn.BackColor = Color.Green;
            btn.ForeColor = Color.White;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Font = new Font("맑은 고딕", 9, FontStyle.Bold);
        }

   private void btnQR_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                Filter = "CSV/TSV Files (*.csv;*.tsv;*.txt)|*.csv;*.tsv;*.txt",
                Title = "처방 CSV/TSV 파일 열기"
            };

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                DisplayCsvInGroupBox(ofd.FileName);
            }
        }

       private void DisplayCsvInGroupBox(string filePath)
        {
            try
            {
                currentPrescriptionDetails.Rows.Clear();
                Patient.Rows.Clear();

                using (var reader = new StreamReader(filePath, Encoding.GetEncoding("euc-kr")))
                {
                    string headerLine = reader.ReadLine();
                    if (headerLine == null) return;

                    char delimiter = headerLine.Count(c => c == '\t') > headerLine.Count(c => c == ',') ? '\t' : ',';

                    int prescriptionNo = 1;
                    string latestName = "";
                    string latestJumin = "";
                    string latestDate = "";
                    string latestDoctor = "";
                    string latestDays = "";
                    string latestHospital = "";

                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        if (string.IsNullOrWhiteSpace(line)) continue;

                        var parts = line.Split(delimiter);
                        if (parts.Length < 10) continue;

                        // 기본 정보
                        string name = parts[0];            // 고객명
                        string jumin = parts[1];           // 주민번호
                        string date = parts[2];            // 처방일자
                        string hospital = parts[3];        // 병원명
                        string doctor = parts[4];          // 의사명
                        string drugCode = parts[5];        // 약코드
                        string drugName = parts[6];        // 약 이름
                        string dose = parts[7];            // 1회 투약량
                        string timesPerDay = parts[8];     // 1일 투약횟수
                        string days = parts[9];            // 총 투약일수

                        // 계산: 총량 = 1회량 * 1일횟수 * 일수
                        string totalAmount = (float.TryParse(dose, out float d) &&
                                              float.TryParse(timesPerDay, out float t) &&
                                              float.TryParse(days, out float n))
                            ? (d * t * n).ToString("0.##") : "";

                        int amountValue = 0;
                        if (int.TryParse(totalAmount, out int parsedAmount))
                        {
                            amountValue = 100 * parsedAmount;
                        }

                        // 현재 조제내역에 추가
                        currentPrescriptionDetails.Rows.Add(
                            prescriptionNo++.ToString(),
                            drugCode, drugName, dose, timesPerDay, days,
                            totalAmount,
                            "건강보험",
                            100,
                            amountValue
                        );

                        // 조제내역 DB 저장
                        var detail = new PrescriptionDetail
                        {
                            Jumin = jumin,
                            Name = name,
                            Date = date,
                            DrugCode = drugCode,
                            DrugName = drugName,
                            Dose = dose,
                            TimesPerDay = timesPerDay,
                            Days = days,
                            TotalAmount = totalAmount,
                            Insulance = "건강보험",
                            Danga = 100,
                            AmountValue = amountValue
                        };
                        DB.InsertDetail(detail);

                        // 마지막 환자 정보 저장 (환자 1명 기준 파일로 가정)
                        latestName = name;
                        latestJumin = jumin;
                        latestDate = date;
                        latestDoctor = doctor;
                        latestDays = days;
                        latestHospital = hospital;

                        // 환자정보에 추가
                        if (!seenPatientsForInfo.Contains(jumin))
                        {
                            string gender = "N/A";
                            int age = -1;

                            string cleanJumin = jumin.Replace("-", "");

                            if (cleanJumin.Length >= 7)
                            {
                                char genderCode = cleanJumin[6];  

                                int century = 0;
                                switch (genderCode)
                                {
                                    case '1': case '2': century = 1900; break;
                                    case '3': case '4': century = 2000; break;
                                    default: century = 0; break;
                                }

                                gender = (genderCode % 2 == 1) ? "남" : "여";

                                string yearPart = cleanJumin.Substring(0, 2);
                                string monthPart = cleanJumin.Substring(2, 2);
                                string dayPart = cleanJumin.Substring(4, 2);

                                if (int.TryParse(yearPart, out int yy) &&
                                    int.TryParse(monthPart, out int mm) &&
                                    int.TryParse(dayPart, out int dd) &&
                                    century > 0)
                                {
                                    int birthYear = century + yy;
                                    string birthDateStr = $"{birthYear:D4}-{mm:D2}-{dd:D2}";
                                    if (DateTime.TryParseExact(birthDateStr, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime birthDate))
                                    {
                                        DateTime today = DateTime.Today;
                                        age = today.Year - birthDate.Year;
                                        if (birthDate > today.AddYears(-age)) age--; // 생일 안 지났으면 -1
                                    }
                                }
                            }

                            string ageSex = (age != -1 ? age.ToString() : "N/A") + "(" + gender + ")";
                            Patient.Rows.Add(
                                name,
                                jumin,
                                ageSex,
                                date,
                                hospital,
                                "" // 조제시 참고사항
                            );
                            seenPatientsForInfo.Add(jumin);
                        }
                    }

                    // 조제 금액 합계 계산 (모든 행 완료 후)
                    int totalAmountSum = 0;
                    foreach (DataGridViewRow row in currentPrescriptionDetails.Rows)
                    {
                        if (row.Cells[9].Value != null && int.TryParse(row.Cells[9].Value.ToString(), out int amount))
                        {
                            totalAmountSum += amount;
                        }
                    }
                    var summary = new PrescriptionSummary
                    {
                        Jumin = latestJumin,
                        Name = latestName,
                        Date = latestDate,
                        Hospital = latestHospital,
                        Doctor = latestDoctor,
                        TotalAmountSum = totalAmountSum
                    };
                    DB.InsertSummary(summary);
                    //Console.WriteLine($"총 금액: {totalAmountSum}");

                    // 일일환자 리스트에 추가
                    string dailyKey = $"{latestName}_{latestJumin}_{latestDate}";
                    if (!seenPatientsForDailyList.Contains(dailyKey))
                    {
                        dailyPatientList.Rows.Insert(0,
                            latestDate,
                            dailyNo++.ToString(),
                            latestName,
                            latestJumin,
                            latestDoctor,
                            latestDays,
                            totalAmountSum
                        );
                        seenPatientsForDailyList.Add(dailyKey);
                    }

                    // 과거 조제 리스트 표시
                    pastPrescriptionList.Rows.Clear();
                    var summaries = DB.GetSummariesByJumin(latestJumin);
                    if (DateTime.TryParse(latestDate, out DateTime csvDate))
                    {
                        int i = 1;
                        foreach (var s in summaries)
                        {
                            if (DateTime.TryParse(s.Date, out DateTime summaryDate) && summaryDate < csvDate)
                            {
                                pastPrescriptionList.Rows.Add(i++, s.Date, s.Hospital, s.Doctor, s.TotalAmountSum);
                            }
                        }
                    }
                    previousPrescriptionDetails.Rows.Clear();
                    var details = DB.GetDetailsByJumin(latestJumin);
                   if (DateTime.TryParse(latestDate, out csvDate)) 
                    {
                        int j = 1;
                        foreach (var d in details)
                        {
                            if (DateTime.TryParse(d.Date, out DateTime recordDate) && recordDate < csvDate)
                            {
                                previousPrescriptionDetails.Rows.Add(j++, d.Date, d.DrugCode, d.DrugName, d.Dose, d.TimesPerDay, d.Days,
                                                                     d.TotalAmount, d.Insulance, d.Danga, d.AmountValue);
                            }
                        }
                    }
                    currentPrescriptionDetails.Refresh();
                    dailyPatientList.Refresh();
                    Patient.Refresh();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"파일을 불러오는 중 오류 발생: {ex.Message}");
            }
        }
    }
}
