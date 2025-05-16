# Upharmacy
<br />

## 1. 소개
> 환자의 약 처방 기록을 시각화하고, 투약 이력을 효율적으로 관리할 수 있는 <br />
> Windows Forms 기반의 데스크탑 애플리케이션입니다.


<br /> <br />
![UPharmacy-2025-05-15-19-26-08](https://github.com/user-attachments/assets/e3ad1a50-2bfb-4b1f-b51b-e8123025a341)
<br /> <br />

### 작업기간
2025/05, 1주
<br /><br />

### 인력구성
1인
<br /><br /><br />

## 2. 기술스택

<img src ="https://img.shields.io/badge/C_sharp-003545.svg?&style=for-the-badge&logo=Csharp&logoColor=brown"/> <img src="https://img.shields.io/badge/mysql-4479A1?style=for-the-badge&logo=mysql&logoColor=white">  <br /><br /> <br />

## 3. 기능
### 📂 Project Structure (폴더 구조)
```
Graph_Drug/
|     
|ㅡ Form1.cs            # UI 및 이벤트 핸들링
|ㅡ DB.cs               # MySQL 데이터베이스 핸들링    
|ㅡ RoundedGroupBox.cs  # 시각화 로직 
└── README.md           # GitHub 설명 파일
```
<br /><br />

## 4. 상세페이지 

- **일일환자 리스트**
  
 ![image](https://github.com/user-attachments/assets/c3dd7315-b78b-4f76-9894-106e2681b976)

<br />

- **과거조제 리스트**

![image](https://github.com/user-attachments/assets/18e7288c-f4c3-420e-b355-f434a2c271dc)

<br />

- **환자정보**

![image](https://github.com/user-attachments/assets/9ee9b9a3-89dd-4da3-8943-7315d37ba18b)

<br />  

- **현재 조제 내역**

![image](https://github.com/user-attachments/assets/24dcf14d-58a6-4093-82e5-bdb68d5b5239)

<br /> 

- **과거 조제 내역**

![image](https://github.com/user-attachments/assets/e2d796c6-e7f3-4cad-9f6d-ce551ab7d840)

<br /> 


<br /><br /> 


## 5. 아쉬웠던 부분
**- 중복 조제 내역 처리 미구현**  <br />
과거 조제 내역 중 동일한 처방이 반복되는 경우,  한 번만 표시되도록 중복 제거 기능을 <br /> 
구현하고자 했으나, 개발 기간의 제약으로 반영하지 못했습니다. <br /> 

**- 보험 기준 약제비 계산 미구현** <br />
실사용자 관점에서 유용한 보험 적용 후 약제비 계산 기능을 구상했으나, <br /> 
관련 정책 적용 로직과 계산 기준을 구현하지 못했습니다. <br /> 

**- 메모 및 참고사항 기능 미구현**  <br /> 
처방전에 포함된 ‘조제 시 참고사항’을 저장할 수 있는 기능을 고려했으나,  <br /> 
완성도를 높이려고 하다보니 초기 개발 범위에서 제외되었습니다.
   
<br /><br /> <br /> 

## 6. 앞으로 학습할 것들, 나아갈 방향
- 정렬, 검색 기능 추가
- 메모 기능 구현
  
<br /><br /> <br /> 
