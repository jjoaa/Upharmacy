using System;
using System.Data.SQLite;
using System.Drawing;
using System.Windows.Forms;

namespace UPharmacy
{
    public class DBTab : TabPage
    {
        private Button btnInsert;
        private Button btnShowData;
        private ListBox listBoxData;

        public DBTab() : base("DB")
        {
            // 초기화
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            // 데이터 삽입 버튼 설정
            btnInsert = new Button
            {
                Text = "데이터 삽입",
                Location = new Point(20, 20),
                Size = new Size(100, 30)
            };
            btnInsert.Click += BtnInsert_Click;  // 클릭 시 데이터 삽입

            // 데이터 조회 버튼 설정
            btnShowData = new Button
            {
                Text = "데이터 조회",
                Location = new Point(150, 20),
                Size = new Size(100, 30)
            };
            btnShowData.Click += BtnShowData_Click;  // 클릭 시 데이터 조회

            // ListBox 설정 (조회된 데이터를 표시)
            listBoxData = new ListBox
            {
                Location = new Point(20, 70),
                Size = new Size(1050, 500)
            };

            // 탭에 버튼과 ListBox 추가
            this.Controls.Add(btnInsert);
            this.Controls.Add(btnShowData);
            this.Controls.Add(listBoxData);
        }

        // 데이터 삽입 이벤트 처리
        private void BtnInsert_Click(object sender, EventArgs e)
        {
            // 데이터 삽입 예시
            //DB.InsertPastPrescription();
            //DB.InsertPrescriptionDetail();

            MessageBox.Show("데이터 삽입 완료");
        }

        // 데이터 조회 이벤트 처리
        private void BtnShowData_Click(object sender, EventArgs e)
        {
            // ListBox에 데이터를 표시
            listBoxData.Items.Clear();  // 기존 목록 초기화
           // DB.GetPastPrescriptions(listBoxData);  // DB에서 조회하여 ListBox에 추가
        }

        public void RefreshData()
        {
            listBoxData.Items.Clear();
           // DB.GetPastPrescriptions(listBoxData);
        }
    }
}
