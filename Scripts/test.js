// JScript File




//////////////////////////age calculation///////////////////////////////////
//to calculate the age based on the date of birth entered in the UI
//this is used in the Step 1 of the Camper Application
function getAge(DOBtxt)
{

    var dob = trim(DOBtxt.value);
    //to find the txtage text box in the page
    var inputobjs, txtageobj;
    inputobjs = document.getElementsByTagName("input");
    for (var i = 0; i<inputobjs.length-1; i++)
    {
        if (inputobjs[i].type=="text" && inputobjs[i].id.indexOf("txtAge")>=0)
        {
            txtageobj= inputobjs[i];
            break;
        }
    }
    
    if (dob !="")
    {
        //if the date is valid then find out the age
       
        if (ValidateDate(dob))
        {
            var datearr = new Array();
            var Day, Month, Year, days, gdate, gmonth, gyear, age;
            datearr = dob.split("/");
            Month = datearr[0];
            Day = datearr[1];
            Year = datearr[2];
        
            days = new Date();
            gdate = days.getDate();
            gmonth = days.getMonth()+1;
            gyear = days.getYear();            

            if (gyear < 1900) { gyear+=1900; }
            age=gyear-Year;
            if(gmonth < Month || (gmonth == Month && gdate < Day)) {age=age-1;}
            
            txtageobj.value=parseInt(age);
        
        }//close tag of if (ValidateDate(dob))
        else
            txtageobj.value="";
    }// close tag of dob !=""
    else
        txtageobj.value="";
}

//to validate a date
function ValidateDate(DateString)
{
    if (trim(DateString)!="")
    {
        var datearr = new Array();
        var Day, Month, Year, days, gdate, gmonth, gyear, age;
        datearr = DateString.split("/");
        Month = datearr[0];
        Day = datearr[1];
        Year = datearr[2];
        
        if ((Month<1) || (Month>12) || isNaN(Month) || (Day<1) || (Day>31) ||isNaN(Day) || (Year<1900) || isNaN(Year) ||(Month=="") || (Day=="") || (Year=="")) 
            return false;
        else if (((Month==4) || (Month==6) || (Month==9) || (Month==11)) && (Day>30)) 
            return false;
        else if (Month==2) 
        {
            if (Day >29)
                return false;
            else if((Day>28) && (!leapyear(Year)))
                return false;
        }
        else if((Year>9999)||(Year<0))
            return false;
            
        return true;
    }
}


//to compare start date and end date and return true if end date is greater than start date, else false
function CompareDates(StartDate, EndDate)
{
    var startDate = new Date(StartDate);
    var endDate = new Date(EndDate);
    //alert(startDate +" ,"+ endDate);
//    var datestring1,datestring2;    
//    datestring1 = StartDate.split("/");
//    var date1 = new Date(datestring1[2],datestring1[0], datestring1[1]) //yy, mm, dd    
//    datestring2 = EndDate.split("/");
//    var date2 = new Date(datestring2[2],datestring2[0], datestring2[1]) //yy, mm, dd
    if (endDate > startDate)
        return true;  //end date is greater than start date
    else
        return false; //end date is lesser than start date
}

            


/////////////////////////////leap year validation////////////////////////////
//to check whether the year is a leap year or not///////////////////////////
function leapyear(a) {
    if(((a%4==0)&&(a%100!=0))||(a%400==0)) return true
    else 
    return false
    }

/////////////////////to trim a string/////////////////////////////
function trim(InputStr)
{
    return InputStr.replace(/^\s*/, "").replace(/\s*$/, "");
}

////////////////to enable / disable the camper school text box if the school type is HOME SCHOOL///////
function setSchoolTextBoxStatus(Radioobj, SchoolTextBoxPanel)
{
    var bDisable;
    var SchoolPanel = document.getElementById(SchoolTextBoxPanel);
    var SchoolPanelChildNodes = SchoolPanel.childNodes;
    switch (Radioobj.value)
    {
        case "3": //Home School
            bDisable = true;
            break;
        default:
            bDisable = false;
            break;
    }
    //to enable/disable the school panel and the controls within it
    SchoolPanel.disabled = bDisable;
    for (var i = 0; i<=SchoolPanelChildNodes.length-1; i++)
    {
        //to enable/disable the control
        if (SchoolPanelChildNodes[i].id != undefined)
            SchoolPanelChildNodes[i].disabled = bDisable;
        
        //to clear the text box value if any    
        if (SchoolPanelChildNodes[i].type == "text")
            SchoolPanelChildNodes[i].value = "";
    }
}

//to open up a calendar
function ShowCalendar(txtBoxId)
{
    window.open('../../Calendar.aspx?txtBox=' + txtBoxId,'Calendar','toolbar=no,status=no,titlebar=no,scroll=no,width=200,height=190,left=610,top=420');
    //window.showModalDialog('../../Calendar.aspx?txtBox=' + txtBoxId,'Calendar','toolbar=no,status=no,titlebar=no,scroll=no,width=200,height=190,left=610,top=420');
    return false;
}

///////////////////////to validate the Step 2 - Page 1 (Question1) of the quesitonnaire/////////////
//TV: 10/2009 - increased size of array because a new question was added and updated questions to reflect the text
var HelpText = new Array(9);
HelpText[0] = "How did you hear about it?";
HelpText[1] = "Which organization, city, state?";
HelpText[2] = "Which website/Email from whom?";
HelpText[3] = "From where?";
HelpText[4] = "Which camp?";
HelpText[5] = "Please specify";
HelpText[6] = "Which publication?";
HelpText[7] = "What station?";
HelpText[8] = "What station?";

function SetHelpText(OptionId, txtObj)
{
    var txtvalue = trim(txtObj.value);
    if (txtvalue == "")
        txtObj.value = HelpText[OptionId];
}
function ClearHelpText(OptionId, txtObj)
{
    var txtvalue = trim(txtObj.value);
    if (txtvalue == HelpText[OptionId])
        txtObj.value = "";            
}

/////////////////to validate the Checkbox question for Cincinnati/Middlesex/Greensboro (Page 1 of step 2 of Cincinati)/////////////////////
function ValidateHowDidYouHearUsPage(sender, args) {
    debugger;
    var inputobjs = new Array();
    var txtbox, bChecked=false, bValid=false, bReferralCode=true;
    var strErrorMsg;
    var valObj = document.getElementById(sender.id);
    valObj.innerText = "";
    inputobjs = document.getElementsByTagName("input");
    var message = "<li>Please check any one option for Question 1</li>";
    var spanobjs = document.getElementsByTagName("span");
    var lblErrorMessage;
    
    for (var i = 0; i < spanobjs.length - 1; i++)
    {
        if(spanobjs[i].id.indexOf("lblErrorMessage") >= 0)
        {
            lblErrorMessage = document.getElementById(spanobjs[i].id);
            lblErrorMessage.style.display = 'none';
        }
    }

    // 2012-09-23 Check three main questions
    // Question 1
    var selectObjs = document.getElementsByTagName("select"),
        Q1WhatYear, Q2Research, Q3aStaffNames;
    debugger;

    for (var i = 0; i <= selectObjs.length - 1; i++) {
        if (selectObjs[i].id.indexOf("ddlWhatYear") >= 0) {
            Q1WhatYear = selectObjs[i];
        } else if (selectObjs[i].id.indexOf("ddlResearch") >= 0) {
            Q2Research = selectObjs[i];
        } else if (selectObjs[i].id.indexOf("ddlStaffNames") >= 0) {
            Q3aStaffNames = selectObjs[i];
        }        
    }

    if (Q1WhatYear.selectedIndex == 0) {
        valobj.innerHTML = "<li>Please select from Question 1</li>";
        args.IsValid = false;
        return;
    }

    if (Q2Research.selectedIndex == 0) {
        valobj.innerHTML = "<li>Please select from Question 2</li>";
        args.IsValid = false;
        return;
    }

    var isQ3bPass = true;
    for (var i = 0; i < inputobjs.length - 1; i++) {
        if ((inputobjs[i].id.indexOf("chkStaff1") >= 0 || inputobjs[i].id.indexOf("chkStaff2") >= 0 || inputobjs[i].id.indexOf("chkStaff3") >= 0) && inputobjs[i].checked) {
            bChecked = true; //a checkbox is checked
            if (Q3aStaffNames.selectedIndex == 0) {
                valobj.innerHTML = "<li>Please select from Question 3a</li>";
                args.IsValid = false;
                return;
            }
        }

        if (inputobjs[i].id.indexOf("chkHearFromAd") >= 0 && inputobjs[i].checked) {
            isQ3bPass = false;
            bChecked = true; //a checkbox is checked
            for (var j = 0; j < inputobjs.length - 1; j++) {
                if (inputobjs[i].id.indexOf("chkStaff1") >= 0 && inputobjs[i].checked) {
                    isQ3bPass = true;
                }
            }
        }
    }
    
    if (inputobjs[i].id.indexOf("ddlWhatYear") >= 0) {
        ddl = document.getElementById("")
    }
    
    if(!bReferralCode)
    {
        args.IsValid=false;
        valObj.innerHTML = "<li>Please enter referral code</li>"        
        return;
    }    
    else if (!bChecked)
    {
        args.IsValid=false;
        valObj.innerHTML = message;
        return;
    }
    
    if (!isQ3bPass) {
        args.IsValid=false;
        valObj.innerHTML = "<li>Please select from Question 3b</li>";
        return;        
    }
    
    if (!bValid)
    {
        valObj.innerHTML = strErrorMsg;
    }
   
    args.IsValid = bValid;
    return;
}



/////////////////to validate the Checkbox question for Cincinnati/Middlesex/Greensboro (Page 1 of step 2 of Cincinati)/////////////////////
function ValidateQ1New(sender,args)
{
    var inputobjs = new Array();
    //var textareaobj = new Array();
    //var AdminDiv, AdmintxtComments;
    var txtbox, bChecked=false, bValid=false;
    var strErrorMsg;
    var valObj = document.getElementById(sender.id);
    valObj.innerText = "";
    inputobjs = document.getElementsByTagName("input");
    
    for(var i=0; i< inputobjs.length-1; i++)
    {
        //A referral from a friend
        if (inputobjs[i].id.indexOf("chk1") >=0 && inputobjs[i].checked)
        {
            txtbox = document.getElementById(inputobjs[i].id.replace("chk1","txt1"));
            bChecked=true; //a checkbox is checked
            
            if (trim(txtbox.value)=="")
            {
                strErrorMsg = "<li>Please enter your friend email address</li>";
                txtbox.focus();
            }
            else
                bValid = true;
            
        }  //A Jewish organization
        else if (inputobjs[i].id.indexOf("chk2") >=0 && inputobjs[i].checked)
        {
            txtbox = document.getElementById(inputobjs[i].id.replace("chk2","txt2"));
            bChecked=true; //a checkbox is checked
            if (trim(txtbox.value)=="")
            {
                strErrorMsg = "<li>Please enter a Jewish Organisation</li>";
                txtbox.focus();
            }
            else
                bValid = true;
        } //A web site
        else if (inputobjs[i].id.indexOf("chk3") >=0 && inputobjs[i].checked)
        {
            txtbox = document.getElementById(inputobjs[i].id.replace("chk3","txt3"));
            bChecked=true; //a checkbox is checked
            if (trim(txtbox.value)=="")
            {
                strErrorMsg = "<li>Please enter the website URL</li>";
                txtbox.focus();
            }
            else
                bValid = true;
        }//An Advertisement
        else if (inputobjs[i].id.indexOf("chk4") >=0 && inputobjs[i].checked)
        {
            txtbox = document.getElementById(inputobjs[i].id.replace("chk4","txt4"));
            bChecked=true; //a checkbox is checked
            if (trim(txtbox.value)=="")
            {
                strErrorMsg = "<li>Please enter where you saw the Advertisement</li>";
                txtbox.focus();
            }
            else
                bValid = true;
        }//A Poster or a Post Card
        else if (inputobjs[i].id.indexOf("chk5") >=0 && inputobjs[i].checked)
        {
            txtbox = document.getElementById(inputobjs[i].id.replace("chk5","txt5"));
            bChecked=true; //a checkbox is checked
            if (trim(txtbox.value)=="")
            {
                strErrorMsg = "<li>Please enter where you saw the Poster / Post Card</li>";
                txtbox.focus();
            }
            else
                bValid=true;
        }//Other
        else if (inputobjs[i].id.indexOf("chk6") >=0 && inputobjs[i].checked)
        {
            txtbox = document.getElementById(inputobjs[i].id.replace("chk6","txt6"));
            bChecked=true; //a checkbox is checked
            if (trim(txtbox.value)=="")
            {
                strErrorMsg = "<li>Please enter the other reason</li>";
                txtbox.focus();
            }
            else
                bValid = true;
        }
        else   //to validate the comments section (only for Admin)
        {
        }
        
        if (inputobjs[i].id.indexOf("chk6") >=0 && inputobjs[i].checked)
        {
            var txt = document.getElementById(inputobjs[i].id.replace("chk6","txt6"));
            if (txt.value == "Please specify")
            {
                bValid = false;  
                strErrorMsg = "<li>Please specify other</li>";
                txtbox.focus();
            }
        }
    } //end of for loop
    
    if (!bChecked)
    {
        args.IsValid=false;
        valObj.innerHTML = "<li>Please check any one option for Question 1</li>"
        return;
    }
    if (!bValid)
    {
        valObj.innerHTML = strErrorMsg;
    }
   
    args.IsValid = bValid;
    return;
}

//to get the Admin text box from the page
function getAdminTextBox()
{
    var textareaobj = new Array();
    textareaobj = document.getElementsByTagName("textarea");
    var AdmintxtComments;
    //to get the admin txtcomments text box
    for (var j=0; j<= textareaobj.length-1; j++)
    {
        if (textareaobj[j].id.indexOf("txtComments")>=0)
        {
            AdmintxtComments = textareaobj[j];
            return AdmintxtComments;
            break;
        }
    }
    return null;
}


//for Question 9 for cincinaati and Question 7 for Middle sex, Q12 for Greenboro
//in Step 2 (Page 3) of Cincinati / Middle sex / Greensboro ///////////////////////
function windowopen()
{
    var arguments = windowopen.arguments;
    var RadioObj, k=0;
    var divctlsid = new Array();
    var bCheckedValue;
    
    //to get the input parameters passed to this method
    //1st parameter - Radiobutton obj
    //rest parameters - divobjects separated by comma
    for (var j=0; j<=arguments.length-1; j++)
    {
        switch (j)
        {
            case 0:
                RadioObj = arguments[j];
                break;
            default:
                divctlsid[k] = arguments[j];
                k=k+1;
                break;
        }
    }

    switch (RadioObj.value)
    {
        case "1":
            bCheckedValue=true;
            break;
        default:
            bCheckedValue = false;
            break;
    }
    
    
    //PanelStatus(windowopen.arguments);
     //to open up the info page
     
    if (bCheckedValue) 
        window.open('../../CampSearch_New.aspx','search','toolbar=no,status=no,scroll=no,width=800,height=400')
    
    
    var divobjs = new Array();
    divobjs = document.getElementsByTagName("div");
    
    //to disable the child nodes
    for (var i=0; i < divobjs.length; i++)
    {
        //to disable all the childnode of the panel        
        for (var l=0; l<=divctlsid.length; l++)
        {   
            //alert(divobjs[i].id.indexOf(divctlsid[l]) + " ," + divobjs[i].id + " ," + divctlsid[l] );
            if(divobjs[i].id.indexOf(divctlsid[l])>=0)
            {                
                divobjs[i].disabled=bCheckedValue;
                var child = new Array();
                child = divobjs[i].childNodes;
                for (var j=0; j<child.length-1; j++)
                {   
                    if (child[j].id !=undefined) 
                    {
                        child[j].disabled = bCheckedValue;
                        //to set the value in the textbox and dropdown to the default value
                        if (child[j].type!=null && bCheckedValue==true)
                        {
                            if (child[j].type.indexOf("select")>=0) child[j].selectedIndex=0; //if it is dropdown
                            else child[j].value=""; //for text box
                        }
                        //this is used for Jwest (Step 2 - Page 3 where the start date and end date are displayed in labels)
                        if (child[j].id.indexOf("StartDate")>=0 || child[j].id.indexOf("EndDate")>=0)
                        {
                            if (bCheckedValue) child[j].innerText="";
                        }
                    }
                }
            }
        }
    }
} 

//***************************VALIDATION FOR CINCINNATI*********************************************

//to validate the Step2 (Page 2) for Cincinatti/////////////////////////
function ValidatePage2Step2_CN(sender,args)
{
    var inputobjs = document.getElementsByTagName("input");
    var Q3_1, Q3_2, Q4_1, Q4_2, Q5_1, Q5_2, Q6, Q7, Q8;
    var Q7 = new Array();
    var j=0;
    var valobj = document.getElementById(sender.id);
    var hdnYearCount;
    for (var i = 0; i< inputobjs.length-1; i++)
    {
        //for Q3_1
        if (inputobjs[i].id.indexOf("RadioBtnQ31")>=0)
        {
            Q3_1 = inputobjs[i];
        }
        //for Q3_2
        if (inputobjs[i].id.indexOf("RadioBtnQ32")>=0)
        {
            Q3_2 = inputobjs[i];
        }
        
        //for Q4_1
        if (inputobjs[i].id.indexOf("RadioBtnQ41")>=0)
        {
            Q4_1 = inputobjs[i];
        }
        
        //for Q4_2
        if (inputobjs[i].id.indexOf("RadioBtnQ42")>=0)
        {
            Q4_2 = inputobjs[i];
        }
        
        //for Q5_1
        if (inputobjs[i].id.indexOf("txtYear")>=0)
        {
            Q5_1 = inputobjs[i];
        }
        
        //for Q5_2
        if (inputobjs[i].id.indexOf("txtNoofDays")>=0)
        {
            Q5_2 = inputobjs[i];
        }
        
        //for Q6
        if (inputobjs[i].id.indexOf("txtGrade")>=0)
        {
            Q6 = inputobjs[i];
        }
        
        //for Q7
        if (inputobjs[i].id.indexOf("RadioButtionQ7")>=0)
        {
            Q7[j] = inputobjs[i];
            j=j+1;
        }
        
        //for Q8
        if (inputobjs[i].id.indexOf("txtCamperSchool")>=0)
        {
            Q8 = inputobjs[i];
        }
        
        if (inputobjs[i].id.indexOf("hdnAddMoreYearCount")>=0)
        {
            hdnYearCount = inputobjs[i];
        }
    }  //end of for loop

    //validate Q3
    if (Q3_1.checked==false && Q3_2.checked==false)
    {
        valobj.innerHTML = "<ul><li>Please answer Question No. 3</li></ul>";
        args.IsValid=false;
        return;
    }
    else if (Q3_2.checked)  //if "no" is checked
    {
        //to validate for Q4 and Q5
        if (Q4_1.checked==false && Q4_2.checked==false)
        {
             valobj.innerHTML = "<li>Please answer Question No. 4</li>";
             args.IsValid=false;
             return;
        }
        else if (Q4_1.checked)
        {
            
            if (hdnYearCount.value==0 && (trim(Q5_1.value)=="" || trim(Q5_2.value)==""))
            {
                 valobj.innerHTML = "<li>Please enter both Year and No of Days for Question No. 5</li>";
                 args.IsValid=false;
                 return;
            }
            else if (hdnYearCount.value>0)
            {
                if ((trim(Q5_1.value)=="" && trim(Q5_2.value)!="") || (trim(Q5_1.value)!="" && trim(Q5_2.value)==""))
                {
                     valobj.innerHTML = "<li>Please enter both Year and No of Days for Question No. 5</li>";
                     args.IsValid=false;
                     return;
                }
            }
        }
        
    }
    
    //validate Q6
    if (trim(Q6.value)=="" || isNaN(trim(Q6.value)))
    {
        valobj.innerHTML = "<li>Please enter a valid Grade</li>";
        args.IsValid=false;
        return;
    }
    else
    {
        //validate Q7
        var bChecked=false;
         
        for (var k=0; k<=Q7.length-1; k++)
        {
            if (Q7[k].checked==true)
            {
                bChecked = true;
            }
        }
        
        if (!bChecked)
        {
            valobj.innerHTML = "<li>Please answer Question No. 7</li>";
            args.IsValid=false;
            return;
        }
        else if (trim(Q8.value)=="")//validate Q8
        {
            valobj.innerHTML = "<li>Please enter Name of the School</li>";
            args.IsValid=false;
            return;
        }
    }   

    args.IsValid = true;
    return;
}


function ValidateNLCamp(sender,args)
{
    var Q_Camp;
    
    var bValid=true;
    var strErrorMsg="";
    
    var selectobjs = document.getElementsByTagName("select");
    var valObj = document.getElementById(sender.id);
    
        //to select the dropdown objs
    for (var j=0; j<=selectobjs.length-1; j++)
    {
        if (selectobjs[j].id.indexOf("ddlCamp")>=0)
            Q_Camp = selectobjs[j];       
    }
    
    if (Q_Camp.selectedIndex==0)
    {
        strErrorMsg="<li>Please select a Camp</li>";
        bValid = false;
    }
    else
    {
        bValid = true;
    }
    
    valObj.innerHTML = strErrorMsg;
    args.IsValid = bValid;
    return;
    
}



//to validate the Step2 (Page 3) of Cincinati Questionaire/////////////////////////
function VaildatePage3Step2(sender,args)
{
    var Q9_1, Q9_2, Q9_3, Q9_4, Q10_State, Q10_Camp, Q11_CampSession, Q12_StartDate, Q12_EndDate;
    var inputobjs = document.getElementsByTagName("input");
    var selectobjs = document.getElementsByTagName("select");
    var valObj = document.getElementById(sender.id);
    var bValid=false;
    var strErrorMsg="";
    
    var startDate = document.getElementById("ctl00_hdnCampSessionStartDate");
    var endDate = document.getElementById("ctl00_hdnCampSessionEndDate");
    var campSeasonErrorMessage = document.getElementById("ctl00_hdncampSeasonErrorMessage");

    for (var i = 0; i<inputobjs.length-1; i++)
    {
        if (inputobjs[i].id.indexOf("RadioButtonQ9Option1")>=0)
            Q9_1 = inputobjs[i];
        else if (inputobjs[i].id.indexOf("RadioButtonQ9Option2")>=0)
            Q9_2 = inputobjs[i];
        else if (inputobjs[i].id.indexOf("RadioButtonQ9Option3")>=0)
            Q9_3 = inputobjs[i];
        else if (inputobjs[i].id.indexOf("RadioButtonQ9Option4")>=0)
            Q9_4 = inputobjs[i];
        else if (inputobjs[i].id.indexOf("RadioButtonQ9Option2")>=0)
            Q9_2 = inputobjs[i];
        else if (inputobjs[i].id.indexOf("txtCampSession")>=0)
            Q11_CampSession = inputobjs[i];  
        else if (inputobjs[i].id.indexOf("txtStartDate")>=0)
            Q12_StartDate = inputobjs[i];
        else if (inputobjs[i].id.indexOf("txtEndDate")>=0)
            Q12_EndDate = inputobjs[i];
    } //end of for loop
    
    //to select the dropdown objs
    for (var j=0; j<=selectobjs.length-1; j++)
    {
        if (selectobjs[j].id.indexOf("ddlState")>=0)
            Q10_State = selectobjs[j];
        else if (selectobjs[j].id.indexOf("ddlCamp")>=0)
            Q10_Camp = selectobjs[j];
    }
   
    //validation for Question 9
    if (Q9_1.checked)
        bValid=true;
    
    else if (Q9_2.checked || Q9_3.checked || Q9_4.checked)
    {
        Q11_CampSession.value = trim(Q11_CampSession.value);
        Q12_StartDate.value = trim(Q12_StartDate.value);
        Q12_EndDate.value = trim(Q12_EndDate.value);
        //validation for the rest of the questions
        //for Question 10 
        if (Q10_Camp.selectedIndex==0)
        {
            strErrorMsg="<li>Please select a Camp</li>";
            bValid = false;
        }
        else if (Q11_CampSession.value=="") //for Question 11
        {
            strErrorMsg="<li>Please enter a Camp Session</li>";
            bValid = false;
        }
        else if (Q12_StartDate.value=="" || Q12_EndDate.value=="") //for Question 12
        {
            strErrorMsg="<li>Please enter dates in the mm/dd/yyyy format, or select the dates by clicking the calendar icons.</li>";
            bValid = false;
        }
        else if (!ValidateDate(Q12_StartDate.value))
        {
            strErrorMsg="<li>Please enter a Valid Start Date in the mm/dd/yyyy format, or select the dates by clicking the calendar icons.</li>";
            bValid = false;
        }
        else if (!ValidateDate(Q12_EndDate.value))
        {
            strErrorMsg="<li>Please enter a Valid End Date in the mm/dd/yyyy format, or select the dates by clicking the calendar icons.</li>";
            bValid = false;
        }
        else if (!CompareDates(Q12_StartDate.value,Q12_EndDate.value))
        {
            strErrorMsg="<li>Start Date should be less than the End Date</li>";
            bValid = false;
        }
        //Added by Ram (10/15/2009) related to allow "May, Jun, Jul, Aug, Sep" as session months
        
        else if (!CompareDates(startDate.value,Q10_StartDate.value))
        {            
            strErrorMsg="<li>"+campSeasonErrorMessage.value+"</li>";
            bValid = false;
        }
        else if (!CompareDates(Q10_StartDate.value,endDate.value))
        {   
            strErrorMsg="<li>"+campSeasonErrorMessage.value+"</li>";
            bValid = false;
        }
        else if (!CompareDates(startDate.value,Q10_EndDate.value))
        {   
            strErrorMsg="<li>"+campSeasonErrorMessage.value+"</li>";
            bValid = false;
        }
        else if (!CompareDates(Q10_EndDate.value,endDate.value))
        {   
            strErrorMsg="<li>"+campSeasonErrorMessage.value+"</li>";
            bValid = false;
        }
        else
            bValid = true;
    }//end of else if
    else  //Question 9 is not answered
    {
        strErrorMsg="<li>Please answer Question No. 9</li>";
        bValid = false;
    }
    valObj.innerHTML = strErrorMsg;
    args.IsValid = bValid;
    return;
}

//***************************END OF VALIDATION FOR CINCINNATI*********************************

//**************************VALIDATION FOR MIDDLESEX QUESTIONNAIRE****************************

////////////////to validate the Step2 (Page 2) for Middlesex/////////////////////////
function ValidatePage2Step2_MidSex(sender,args)
{
    var inputobjs = document.getElementsByTagName("input");
    var selectobjs = document.getElementsByTagName("select");
    var Q3_1, Q3_2, Q4, Q5, Q6, bValid = true, Q5CheckedValue;
    var Q5 = new Array();
    var j=0;
    var valobj = document.getElementById(sender.id);
    
    for (var i = 0; i< inputobjs.length-1; i++)
    {
        //for Q3_1
        if (inputobjs[i].id.indexOf("RadioBtnQ31")>=0)
            Q3_1 = inputobjs[i];
        //for Q3_2
        if (inputobjs[i].id.indexOf("RadioBtnQ32")>=0)
            Q3_2 = inputobjs[i];
        //for Q5
        if (inputobjs[i].id.indexOf("RadioButtionQ5")>=0)
        {
            Q5[j] = inputobjs[i];
            j=j+1;
        }
        //for Q6
        if (inputobjs[i].id.indexOf("txtCamperSchool")>=0)
            Q6 = inputobjs[i];
    }  //end of for loop
    
    //to get the select objects (ddlgrade) for Q4
    for (var k=0; k<= selectobjs.length-1; k++)
    {
        if (selectobjs[k].id.indexOf("ddlGrade")>=0)
        {
            Q4 = selectobjs[k];
            break;
        }
    }

    //validate Q3
    if (Q3_1.checked==false && Q3_2.checked==false)
    {
        valobj.innerHTML = "<ul><li>Please answer Question No. 1</li></ul>";
        bValid = false;
    }
    //validate Q4
    else if (Q4.selectedIndex==0)
    {
        valobj.innerHTML = "<ul><li>Please select a Grade</li></ul>";
        bValid = false;
    }
    else
    {
        //validate Q5
        var bChecked=false;
         
        for (var k=0; k<=Q5.length-1; k++)
        {
            if (Q5[k].checked==true)
            {
                Q5CheckedValue = Q5[k].value;
                bChecked = true;
                break;
            }
        }
        
        if (!bChecked)
        {
            valobj.innerHTML = "<ul><li>Please answer Question No. 3</li></ul>";
            bValid = false;
        }
        else if (Q5CheckedValue!="3" && trim(Q6.value)=="")//validate Q6 (if it is not home school)
        {
            valobj.innerHTML = "<ul><li>Please enter Name of the School</li></ul>";
            bValid = false;
        }
    }  

    args.IsValid = bValid;
    return;
}
////////////////to validate the Step2 (Page 2) for Central New Jersey/////////////////////////
function ValidatePage2Step2_CNJ(sender,args)
{
    var inputobjs = document.getElementsByTagName("input");
    var selectobjs = document.getElementsByTagName("select");
    var Q2, Q3, Q4, Q5, Q6, Q7;
    Q2 = new Array();Q4= new Array();Q6 = new Array();
    var j=0;
    var k=0;
    var l=0;
    var m=0;var n=0;var p=0;
    var valobj = document.getElementById(sender.id);
    var b2Checked=false;
    var b4Checked=false;    
    var Q2CheckedValue;
    var Q4CheckedValue;
    for (var i = 0; i< inputobjs.length-1; i++)
    {
        //for Q2
        if (inputobjs[i].id.indexOf("rdnBtnList1")>=0)
        {
            Q2[j] = inputobjs[i];
            j=j+1;
        }
        
        //for Q6
        if (inputobjs[i].id.indexOf("RadioButtonQ5")>=0)
        {
            Q6[m] = inputobjs[i];
            m=m+1;
        }
        //for Q7
        if (inputobjs[i].id.indexOf("txtCamperSchool")>=0)
            Q7 = inputobjs[i];
    }  //end of for loop
    
    //to get the select objects (ddlgrade) for Q5
    for (var k=0; k<= selectobjs.length-1; k++)
    {
        if (selectobjs[k].id.indexOf("ddlGrade")>=0)
        {
            Q3 = selectobjs[k];
            break;
        }        
    } 


    for (var k=0; k<=Q2.length-1; k++)
    {
        if (Q2[k].checked==true)
        {
            Q2CheckedValue = Q2[k].value;
            b2Checked = true;
            break;
        }
    }    
    

    
    if (!b2Checked)
    {
        valobj.innerHTML = "<ul><li>Please answer Question No. 1</li></ul>";
        args.IsValid=false;
        return;
    }
          
    //validate Q3
    if (args.IsValid && Q3.selectedIndex==0)
    {
        valobj.innerHTML = "<li>Please select the Grade</li>";
        args.IsValid=false;
        return;
    } 

   
    
    //validate Q6
    var b6Checked=false;
    var Q6CheckedValue;
    
    for (var k=0; k<=Q6.length-1; k++)
    {
        if (Q6[k].checked==true)
        {
            Q6CheckedValue = Q6[k].value;
            b6Checked = true;
            break;
        }
    }
    if (args.IsValid && !b6Checked)
    {
        valobj.innerHTML = "<ul><li>Please answer Question No. 3</li></ul>";
        args.IsValid=false;
        return;
    }
    else if (args.IsValid && Q6CheckedValue!="3" && trim(Q7.value)=="")//validate Q6 (if it is not home school)
    {
        valobj.innerHTML = "<ul><li>Please enter Name of the School</li></ul>";
        args.IsValid=false;
        return;
    }
    var returnVal = ValidateSynagogueAndJCCQuestion(inputobjs,selectobjs,valobj,false);
    if(returnVal == false)
    {
        args.IsValid = false;
        return;
    }
    args.IsValid = true;
    return;
}
///End of CNJ///
////////////////to validate the Step2 (Page 2) for Northern New Jersey/////////////////////////
function ValidatePage2Step2_NNJ(sender,args)
{
    var inputobjs = document.getElementsByTagName("input");
    var selectobjs = document.getElementsByTagName("select");
    var Q2, Q3, Q4, Q5, Q6, Q7;
    Q2 = new Array();Q4= new Array();Q6 = new Array();
    var j=0;
    var k=0;
    var l=0;
    var m=0;var n=0;var p=0;
    var valobj = document.getElementById(sender.id);
    var b2Checked=false;
    var b4Checked=false;    
    var Q2CheckedValue;
    var Q4CheckedValue;
    for (var i = 0; i< inputobjs.length-1; i++)
    {
        //for Q2
        if (inputobjs[i].id.indexOf("rdnBtnList1")>=0)
        {
            Q2[j] = inputobjs[i];
            j=j+1;
        }
        //for Q3
//        else if (inputobjs[i].id.indexOf("rdnBtnList2")>=0)
//        {
//            Q4[k] = inputobjs[i];
//            k=k+1;
//        }        
                 
        //for Q6
        if (inputobjs[i].id.indexOf("RadioButtonQ5")>=0)
        {
            Q6[m] = inputobjs[i];
            m=m+1;
        }
        //for Q7
        if (inputobjs[i].id.indexOf("txtCamperSchool")>=0)
            Q7 = inputobjs[i];
    }  //end of for loop
    
    //to get the select objects (ddlgrade) for Q5
    for (var k=0; k<= selectobjs.length-1; k++)
    {
        if (selectobjs[k].id.indexOf("ddlGrade")>=0)
        {
            Q3 = selectobjs[k];
            break;
        }        
    } 
//    for (var k=0; k<= selectobjs.length-1; k++)
//    {  
//        if (selectobjs[k].id.indexOf("ddlSynagogue")>=0)
//        {
//            Q5 = selectobjs[k];
//            break;
//        }
//    }   

    for (var k=0; k<=Q2.length-1; k++)
    {
        if (Q2[k].checked==true)
        {
            Q2CheckedValue = Q2[k].value;
            b2Checked = true;
            break;
        }
    }    
    
//    for (var k=0; k<=Q4.length-1; k++)
//    {
//        if (Q4[k].checked==true)
//        {
//            Q4CheckedValue = Q4[k].value;
//            b4Checked = true;
//            break;
//        }
//    }
    
    if (!b2Checked)
    {
        valobj.innerHTML = "<ul><li>Please answer Question No. 1</li></ul>";
        args.IsValid=false;
        return;
    }
          
    //validate Q3
    if (args.IsValid && Q3.selectedIndex==0)
    {
        valobj.innerHTML = "<li>Please select the Grade</li>";
        args.IsValid=false;
        return;
    } 
//    if (args.IsValid && !b4Checked)
//    {
//        valobj.innerHTML = "<ul><li>Please answer Question No. 4</li></ul>";
//        args.IsValid=false;
//        return;
//    }
    
//    if(b4Checked && Q4CheckedValue == "1" &&  args.IsValid && Q5.selectedIndex==0)
//    {
//        valobj.innerHTML = "<ul><li>Please select synagogue.</li></ul>";
//        args.IsValid=false;
//        return;
//    }   
       var returnVal = ValidateSynagogueAndJCCQuestion(inputobjs,selectobjs,valobj,false);
    if(returnVal == false)
    {
        args.IsValid = false;
        return;
    }
    
    //validate Q6
    var b6Checked=false;
    var Q6CheckedValue;
    
    for (var k=0; k<=Q6.length-1; k++)
    {
        if (Q6[k].checked==true)
        {
            Q6CheckedValue = Q6[k].value;
            b6Checked = true;
            break;
        }
    }
    if (args.IsValid && !b6Checked)
    {
        valobj.innerHTML = "<ul><li>Please answer Question No. 4</li></ul>";
        args.IsValid=false;
        return;
    }
    else if (args.IsValid && Q6CheckedValue!="3" && trim(Q7.value)=="")//validate Q6 (if it is not home school)
    {
        valobj.innerHTML = "<ul><li>Please enter Name of the School</li></ul>";
        args.IsValid=false;
        return;
    }

    args.IsValid = true;
    return;
}
//to validate the Step2 (Page 3) of Middlesex Questionaire/////////////////////////
function VaildatePage3Step2_Midsex(sender,args)
{
    var Q7_1, Q7_2, Q7_3, Q7_4, Q8_State, Q8_Camp, Q9_CampSession, Q10_StartDate, Q10_EndDate;
    var inputobjs = document.getElementsByTagName("input");
    var selectobjs = document.getElementsByTagName("select");
    var valObj = document.getElementById(sender.id);
    var dt = new Date();
    var currentDt = dt.getMonth()+ 1 + "/" + dt.getDate() + "/" + dt.getYear();
    var bValid=false;
    var strErrorMsg="";
    var startDate = document.getElementById("ctl00_hdnCampSessionStartDate");
    var endDate = document.getElementById("ctl00_hdnCampSessionEndDate");
    var campSeasonErrorMessage = document.getElementById("ctl00_hdncampSeasonErrorMessage");
    
    for (var i = 0; i<inputobjs.length-1; i++)
    {
        if (inputobjs[i].id.indexOf("RadioButtonQ7Option1")>=0)
            Q7_1 = inputobjs[i];
        else if (inputobjs[i].id.indexOf("RadioButtonQ7Option2")>=0)
            Q7_2 = inputobjs[i];
//        else if (inputobjs[i].id.indexOf("RadioButtonQ7Option3")>=0)
//            Q7_3 = inputobjs[i];
//        else if (inputobjs[i].id.indexOf("RadioButtonQ7Option4")>=0)
//            Q7_4 = inputobjs[i];
        else if (inputobjs[i].id.indexOf("RadioButtonQ7Option2")>=0)
            Q7_2 = inputobjs[i];
        else if (inputobjs[i].id.indexOf("txtCampSession")>=0)
            Q9_CampSession = inputobjs[i];  
        else if (inputobjs[i].id.indexOf("txtStartDate")>=0)
            Q10_StartDate = inputobjs[i];
        else if (inputobjs[i].id.indexOf("txtEndDate")>=0)
            Q10_EndDate = inputobjs[i];
    } //end of for loop
    
    //to select the dropdown objs
    for (var j=0; j<=selectobjs.length-1; j++)
    {
        if (selectobjs[j].id.indexOf("ddlState")>=0)
            Q8_State = selectobjs[j];
        else if (selectobjs[j].id.indexOf("ddlCamp")>=0)
            Q8_Camp = selectobjs[j];
    }
   
    //validation for Question 7
    if (Q7_1.checked)
        bValid=true;
    
    else if (Q7_2.checked )
    {
        Q9_CampSession.value = trim(Q9_CampSession.value);
        Q10_StartDate.value = trim(Q10_StartDate.value);
        Q10_EndDate.value = trim(Q10_EndDate.value);
        //validation for the rest of the questions
        //for Question 10 
        if (Q8_Camp.selectedIndex==0)
        {
            strErrorMsg="<li>Please select a Camp</li>";
            bValid = false;
        }
        else if (Q9_CampSession.value=="") //for Question 11
        {
            strErrorMsg="<li>Please enter a Camp Session</li>";
            bValid = false;
        }
        else if (Q10_StartDate.value=="" || Q10_EndDate.value=="") //for Question 12
        {
            strErrorMsg="<li>Please enter dates in the mm/dd/yyyy format, or select the dates by clicking the calendar icons</li>";
            bValid = false;
        }
        else if (!ValidateDate(Q10_StartDate.value))
        {
            strErrorMsg="<li>Please enter a Valid Start Date in the mm/dd/yyyy format, or select the date by clicking the calendar icon</li>";
            bValid = false;
        }
        else if (!ValidateDate(Q10_EndDate.value))
        {
            strErrorMsg="<li>Please enter a Valid End Date in the mm/dd/yyyy format, or select the date by clicking the calendar icon</li>";
            bValid = false;
        }
        //else if (!CompareDates(currentDt,Q10_StartDate.value))
        //{
        //    strErrorMsg="<li>Start date can not be less than today's date</li>";
        //    bValid = false;
        //}
        else if (!CompareDates(Q10_StartDate.value,Q10_EndDate.value))
        {
            strErrorMsg="<li>Start Date should be less than the End Date</li>";
            bValid = false;
        }
        //Added by Ram (10/15/2009) related to allow "May, Jun, Jul, Aug, Sep" as session months
        
        else if (!CompareDates(startDate.value,Q10_StartDate.value))
        {            
            strErrorMsg="<li>"+campSeasonErrorMessage.value+"</li>";
            bValid = false;
        }
        else if (!CompareDates(Q10_StartDate.value,endDate.value))
        {   
            strErrorMsg="<li>"+campSeasonErrorMessage.value+"</li>";
            bValid = false;
        }
        else if (!CompareDates(startDate.value,Q10_EndDate.value))
        {   
            strErrorMsg="<li>"+campSeasonErrorMessage.value+"</li>";
            bValid = false;
        }
        else if (!CompareDates(Q10_EndDate.value,endDate.value))
        {   
            strErrorMsg="<li>"+campSeasonErrorMessage.value+"</li>";
            bValid = false;
        }
        else
            bValid = true;
    }//end of else if
    else  //Camp registration Question  is not answered
    {
        strErrorMsg="<li>Please answer the Camp registration Question </li>";
        bValid = false;
    }
    
    valObj.innerHTML = strErrorMsg;
    args.IsValid = bValid;
    return;
}

//*****************************END OF VALIDATION FOR MIDDLESEX**********************************

//******************************* START VALIDATION FOR Rhode island **********************************
function ValidatePage2Step2_RhodeIsland(sender,args)
{
    var inputobjs = document.getElementsByTagName("input");
    var selectobjs = document.getElementsByTagName("select");
    var Q3_1, Q3_2, Q4, Q5, Q6, bValid = true, Q5CheckedValue;
    var Q5 = new Array();
    var Synagogue_0,Synagogue_1,SynagogueOther;
    var SynagogueComboValue;
    var j=0;
    var valobj = document.getElementById(sender.id);
    
    for (var i = 0; i< inputobjs.length-1; i++)
    {
        //for Q3_1
        if (inputobjs[i].id.indexOf("RadioBtnQ31")>=0)

            Q3_1 = inputobjs[i];

        //for Q3_2
        if (inputobjs[i].id.indexOf("RadioBtnQ32")>=0)

            Q3_2 = inputobjs[i];


        //for Q5
        if (inputobjs[i].id.indexOf("RadioButtionQ5")>=0)
        {
            Q5[j] = inputobjs[i];
            j=j+1;
        }
        //for Q6
        if (inputobjs[i].id.indexOf("txtCamperSchool")>=0)
            Q6 = inputobjs[i];
            
//        if (inputobjs[i].id.indexOf("RadiobuttonQ4_0")>=0)
//            Synagogue_0 = inputobjs[i];         
//         
//        if (inputobjs[i].id.indexOf("RadiobuttonQ4_1")>=0)
//            Synagogue_1 = inputobjs[i];
//         
//        if(inputobjs[i].id.indexOf("txtOtherSynagogue") >=0)
//            SynagogueOther = inputobjs[i]; 
    }  //end of for loop
    
    //to get the select objects (ddlgrade) for Q4
    for (var k=0; k<= selectobjs.length-1; k++)
    {
        if (selectobjs[k].id.indexOf("ddlGrade")>=0)
        {
            Q4 = selectobjs[k];
            break;
        }
    }

    //to get the select objects (ddlSynagogue) for Q4
//    for (var k=0; k<= selectobjs.length-1; k++)
//    {
//        if (selectobjs[k].id.indexOf("ddlSynagogue")>=0)
//        {
//            SynagogueComboValue = selectobjs[k];
//            break;
//        }
//    }
    
    //validate Q3
    if (Q3_1.checked==false && Q3_2.checked==false)
    {
        valobj.innerHTML = "<ul><li>Please answer Question No. 1</li></ul>";
        bValid = false;
    }


//    else if (Synagogue_0.checked==true && (SynagogueComboValue.selectedIndex==0||( SynagogueComboValue.options[SynagogueComboValue.selectedIndex].text != "Other")|| (trim(SynagogueOther.value)=="") && SynagogueComboValue.options[SynagogueComboValue.selectedIndex].text == "Other") )
//    {
//        if (SynagogueComboValue.selectedIndex==0)
//        {
//            valobj.innerHTML = "<ul><li>Please select a Synagogue</li></ul>";
//            bValid = false;
//        }
//        //added by Ram    
//        else if(SynagogueComboValue.options[SynagogueComboValue.selectedIndex].text == "Other")
//        {
//            if(trim(SynagogueOther.value)=="")
//            {
//                valobj.innerHTML = "<ul><li>Please enter Name of the Synagogue</li></ul>";
//                bValid = false; 
//            }
//        }
//                
//    }
    //validate Q4
    else if (Q4.selectedIndex==0)
    {
        valobj.innerHTML = "<ul><li>Please select a Grade</li></ul>";
        bValid = false;
    }
    else
    {
        //validate Q5
        var bChecked=false;
         
        for (var k=0; k<=Q5.length-1; k++)
        {
            if (Q5[k].checked==true)
            {
                Q5CheckedValue = Q5[k].value;
                bChecked = true;
                break;
            }
        }
        if (!bChecked)
        {
            valobj.innerHTML = "<ul><li>Please answer Question No. 3</li></ul>";
            bValid = false;
        }
        else if (Q5CheckedValue!="3" && trim(Q6.value)=="")//validate Q6 (if it is not home school)
        {
            valobj.innerHTML = "<ul><li>Please enter Name of the School</li></ul>";
            bValid = false;
        }
    }  
    if(bValid)
    {
        var returnVal = ValidateSynagogueAndJCCQuestion(inputobjs,selectobjs,valobj,false);
        if(returnVal == false)
        {
            bValid = false;
        }
    }

    args.IsValid = bValid;
    return;
  
}
//********************************* END OF VALIDATION FOR Rhode island ********************************

//******************************* START VALIDATION FOR MEMPHIS **********************************
function ValidatePage2Step2_Memphis(sender,args)
{
    var inputobjs = document.getElementsByTagName("input");
    var selectobjs = document.getElementsByTagName("select");
    var Q3_1, Q3_2, Q4, Q5, Q6, bValid = true, Q5CheckedValue,Q8_1,Q8_2,Q9_1,Q9_2;
    var Q5 = new Array();
    var Synagogue_0,Synagogue_1,SynagogueOther;
    var SynagogueComboValue;
    var j=0;
    var valobj = document.getElementById(sender.id);
    
    for (var i = 0; i< inputobjs.length-1; i++)
    {
        //for Q3_1
//        if (inputobjs[i].id.indexOf("RadioBtnQ31")>=0)
//            Q3_1 = inputobjs[i];
//        //for Q3_2
//        if (inputobjs[i].id.indexOf("RadioBtnQ32")>=0)
//            Q3_2 = inputobjs[i];
        if (inputobjs[i].id.indexOf("RadioBtnQ3_0")>=0)
        {
            Q3_1 = inputobjs[i];
        }
        //for Q3_2
        if (inputobjs[i].id.indexOf("RadioBtnQ3_1")>=0)
        {
            Q3_2 = inputobjs[i];
        }
             //for Q4_1
        if (inputobjs[i].id.indexOf("RadioBtnQ4_0")>=0)
        {
            Q8_1 = inputobjs[i];
        }
        
        //for Q4_2
        if (inputobjs[i].id.indexOf("RadioBtnQ4_1")>=0)
        {
            Q8_2 = inputobjs[i];
        }
        
        //for Q5_1
        if (inputobjs[i].id.indexOf("RadioBtnQ5_0")>=0)
        {
            Q9_1 = inputobjs[i];
        }
        
        //for Q5_2
        if (inputobjs[i].id.indexOf("RadioBtnQ5_1")>=0)
        {
            Q9_2 = inputobjs[i];
        }
        //for Q5
        if (inputobjs[i].id.indexOf("RadioButtionQ5")>=0)
        {
            Q5[j] = inputobjs[i];
            j=j+1;
        }
        //for Q6
        if (inputobjs[i].id.indexOf("txtCamperSchool")>=0)
            Q6 = inputobjs[i];
            
//        if (inputobjs[i].id.indexOf("RadiobuttonQ4_0")>=0)
//            Synagogue_0 = inputobjs[i];         
//         
//        if (inputobjs[i].id.indexOf("RadiobuttonQ4_1")>=0)
//            Synagogue_1 = inputobjs[i];
//         
//        if(inputobjs[i].id.indexOf("txtOtherSynagogue") >=0)
//            SynagogueOther = inputobjs[i]; 
    }  //end of for loop
    
    //to get the select objects (ddlgrade) for Q4
    for (var k=0; k<= selectobjs.length-1; k++)
    {
        if (selectobjs[k].id.indexOf("ddlGrade")>=0)
        {
            Q4 = selectobjs[k];
            break;
        }
    }

    //to get the select objects (ddlSynagogue) for Q4
//    for (var k=0; k<= selectobjs.length-1; k++)
//    {
//        if (selectobjs[k].id.indexOf("ddlSynagogue")>=0)
//        {
//            SynagogueComboValue = selectobjs[k];
//            break;
//        }
//    }
    
    //validate Q3
    if (Q3_1.checked==false && Q3_2.checked==false)
    {
        valobj.innerHTML = "<ul><li>Please answer Question No. 1</li></ul>";
        bValid = false;
    }
    else if (Q3_2.checked)  //if "no" is checked
    {
        //to validate for Q4 and Q5
        if (Q8_1.checked==false && Q8_2.checked==false)
        {
             valobj.innerHTML = "<li>Please answer Question No. 2</li>";
             bValid = false;
        }
        else if (Q8_1.checked)
        {
            if (Q9_1.checked==false && Q9_2.checked==false)
            {
                 valobj.innerHTML = "<li>Please answer Question No. 3</li>";
                 bValid = false;
            }

        }

    }
//    else if (Synagogue_0.checked==true && (SynagogueComboValue.selectedIndex==0||( SynagogueComboValue.options[SynagogueComboValue.selectedIndex].text != "Other")|| (trim(SynagogueOther.value)=="") && SynagogueComboValue.options[SynagogueComboValue.selectedIndex].text == "Other") )
//    {
//        if (SynagogueComboValue.selectedIndex==0)
//        {
//            valobj.innerHTML = "<ul><li>Please select a Synagogue</li></ul>";
//            bValid = false;
//        }
//        //added by Ram    
//        else if(SynagogueComboValue.options[SynagogueComboValue.selectedIndex].text == "Other")
//        {
//            if(trim(SynagogueOther.value)=="")
//            {
//                valobj.innerHTML = "<ul><li>Please enter Name of the Synagogue</li></ul>";
//                bValid = false; 
//            }
//        }
//                
//    }
    //validate Q4
    if (bValid && Q4.selectedIndex==0)
    {
        valobj.innerHTML = "<ul><li>Please select a Grade</li></ul>";
        bValid = false;
    }
//    else
//    {
        //validate Q5
        var bChecked=false;
         
        for (var k=0; k<=Q5.length-1; k++)
        {
            if (Q5[k].checked==true)
            {
                Q5CheckedValue = Q5[k].value;
                bChecked = true;
                break;
            }
        }
        if (bValid && !bChecked)
        {
            valobj.innerHTML = "<ul><li>Please answer Question No. 5</li></ul>";
            bValid = false;
        }
        else if (bValid && Q5CheckedValue!="3" && trim(Q6.value)=="")//validate Q6 (if it is not home school)
        {
            valobj.innerHTML = "<ul><li>Please enter Name of the School</li></ul>";
            bValid = false;
        }
//    }  
    if(bValid)
    {
        var returnVal = ValidateSynagogueAndJCCQuestion(inputobjs,selectobjs,valobj,false);
        if(returnVal == false)
        {
            bValid = false;
        }
    }

    args.IsValid = bValid;
    return;
  
}
//********************************* END OF VALIDATION FOR MEMPHIS ********************************

//**************************VALIDATION FOR BOSTON QUESTIONNAIRE****************************

////////////////to validate the Step2 (Page 2) for Middlesex/////////////////////////
function ValidatePage2Step2_Boston(sender,args)
{
    var inputobjs = document.getElementsByTagName("input");
    var selectobjs = document.getElementsByTagName("select");
    var Q3_1, Q3_2, Q4, Q5, Q6, bValid = true, Q5CheckedValue;
    var Q5 = new Array();
    var j=0;
    var valobj = document.getElementById(sender.id);
    var Synagogue_0,Synagogue_1,SynagogueOther;
    var SynagogueComboValue;
    var SynRefCode;
    
    for (var i = 0; i< inputobjs.length-1; i++)
    {
        //for Q3_1
        if (inputobjs[i].id.indexOf("RadioBtnQ31")>=0)
            Q3_1 = inputobjs[i];
        //for Q3_2
        if (inputobjs[i].id.indexOf("RadioBtnQ32")>=0)
            Q3_2 = inputobjs[i];
        //for Q5
        if (inputobjs[i].id.indexOf("RadioButtionQ5")>=0)
        {
            Q5[j] = inputobjs[i];
            j=j+1;
        }
        //for Q6
        if (inputobjs[i].id.indexOf("txtCamperSchool")>=0)
            Q6 = inputobjs[i];
            
//        if (inputobjs[i].id.indexOf("txtSynagogueReferral")>=0)
//            SynRefCode= inputobjs[i];   
        
//        if (inputobjs[i].id.indexOf("RadiobuttonQ4_0")>=0)
//            Synagogue_0 = inputobjs[i];         
//         
//        if (inputobjs[i].id.indexOf("RadiobuttonQ4_1")>=0)
//            Synagogue_1 = inputobjs[i];
//         
//        if(inputobjs[i].id.indexOf("txtOtherSynagogue") >=0)
//            SynagogueOther = inputobjs[i]; 
    }  //end of for loop   

    
    //to get the select objects (ddlgrade) for Q4
    for (var k=0; k<= selectobjs.length-1; k++)
    {
        if (selectobjs[k].id.indexOf("ddlGrade")>=0)
        {
            Q4 = selectobjs[k];
            break;
        }
    }
    
    //to get the select objects (ddlSynagogue) for Q4
//    for (var k=0; k<= selectobjs.length-1; k++)
//    {
//        if (selectobjs[k].id.indexOf("ddlSynagogue")>=0)
//        {
//            SynagogueComboValue = selectobjs[k];
//            break;
//        }
//    }
     
    //validate Q3
    
    if (Q3_1.checked==false && Q3_2.checked==false)
    {
        valobj.innerHTML = "<ul><li>Please answer Question No. 1</li></ul>";
        args.IsValid = false;
        return;
    }
    if(Q3_1.checked==true || Q3_2.checked==true)
    {
        var returnVal = ValidateSynagogueAndJCCQuestion(inputobjs,selectobjs,valobj,false);
        if(returnVal == false)
        {
            args.IsValid = false;
            return;
        }
    }
//    else if (Synagogue_0.checked==false && Synagogue_1.checked==false)
//    {
//        valobj.innerHTML = "<ul><li>Please answer Question No. 3</li></ul>";
//        bValid = false;
//    }
//    else if (Synagogue_0.checked==true && (SynagogueComboValue.selectedIndex==0||(trim(SynRefCode.value)=="" && SynagogueComboValue.options[SynagogueComboValue.selectedIndex].text != "Other")|| (trim(SynagogueOther.value)=="") && SynagogueComboValue.options[SynagogueComboValue.selectedIndex].text == "Other") )
//    {
//        if (SynagogueComboValue.selectedIndex==0)
//        {
//            valobj.innerHTML = "<ul><li>Please select a Synagogue</li></ul>";
//            bValid = false;
//        }
//        //added by Ram    
//        else if(SynagogueComboValue.options[SynagogueComboValue.selectedIndex].text == "Other")
//        {
//            if(trim(SynagogueOther.value)=="")
//            {
//                valobj.innerHTML = "<ul><li>Please enter Name of the Synagogue</li></ul>";
//                bValid = false; 
//            }
//        }
//        else if (trim(SynRefCode.value)=="")             
//        {            
//                valobj.innerHTML = "<ul><li>Please enter Synagogue referal code </li></ul>";
//                bValid = false;             
//        }
//        
//    } 
//    else if (Synagogue_0.checked==true && !(trim(SynRefCode.value)==""))
//    {
//        if (!IsNumeric(trim(SynRefCode.value)))
//        {
//            valobj.innerHTML = "<ul><li>Please enter valid referal code </li></ul>";
//            bValid = false;   
//        }   
//    }
//    

    //validate Q4
    if (bValid && Q4.selectedIndex==0)
    {
        valobj.innerHTML = "<ul><li>Please select a Grade</li></ul>";
        args.IsValid = false;
        return;
    }
    else
    {
        
        //validate Q5
        var bChecked=false;
         
        for (var k=0; k<=Q5.length-1; k++)
        {
            if (Q5[k].checked==true)
            {
                Q5CheckedValue = Q5[k].value;
                bChecked = true;
                break;
            }
        }
        
        if (!bChecked)
        {
            valobj.innerHTML = "<ul><li>Please answer Question No. 4</li></ul>";
            args.IsValid = false;
            return;
        }
        else if (Q5CheckedValue!="3" && trim(Q6.value)=="")//validate Q6 (if it is not home school)
        {
            valobj.innerHTML = "<ul><li>Please enter Name of the School</li></ul>";
            args.IsValid = false;
            return;
        }
    }
    
}

//*****************************END OF VALIDATION FOR BOSTON**********************************

//**************************VALIDATION FOR Pittsburgh QUESTIONNAIRE****************************

////////////////to validate the Step2 (Page 2) for Middlesex/////////////////////////
function ValidatePage2Step2_Pittsburgh(sender,args)
{
    var inputobjs = document.getElementsByTagName("input");
    var selectobjs = document.getElementsByTagName("select");
    var Q3_1, Q3_2, Q4, Q5, Q6, bValid = true, Q5CheckedValue;
    var Q5 = new Array();
    var j=0;
    var valobj = document.getElementById(sender.id);
    var Synagogue_0,Synagogue_1,SynagogueOther;
    var SynagogueComboValue;
    var SynRefCode;
    
    for (var i = 0; i< inputobjs.length-1; i++)
    {
        //for Q3_1
        if (inputobjs[i].id.indexOf("RadioBtnQ31")>=0)
            Q3_1 = inputobjs[i];
        //for Q3_2
        if (inputobjs[i].id.indexOf("RadioBtnQ32")>=0)
            Q3_2 = inputobjs[i];
        //for Q5
        if (inputobjs[i].id.indexOf("RadioButtionQ5")>=0)
        {
            Q5[j] = inputobjs[i];
            j=j+1;
        }
        //for Q6
        if (inputobjs[i].id.indexOf("txtCamperSchool")>=0)
            Q6 = inputobjs[i];
            

    }  //end of for loop   

    
    //to get the select objects (ddlgrade) for Q4
    for (var k=0; k<= selectobjs.length-1; k++)
    {
        if (selectobjs[k].id.indexOf("ddlGrade")>=0)
        {
            Q4 = selectobjs[k];
            break;
        }
    }
  
     
    //validate Q3
    
    if (Q3_1.checked==false && Q3_2.checked==false)
    {
        valobj.innerHTML = "<ul><li>Please answer Question No. 1</li></ul>";
        args.IsValid = false;
        return;
    }
    if(Q3_1.checked==true || Q3_2.checked==true)
    {
        var returnVal = ValidateSynagogueAndJCCQuestion(inputobjs,selectobjs,valobj,false);
        if(returnVal == false)
        {
            args.IsValid = false;
            return;
        }
    }

    //validate Q4
    if (bValid && Q4.selectedIndex==0)
    {
        valobj.innerHTML = "<ul><li>Please select a Grade</li></ul>";
        args.IsValid = false;
        return;
    }
    else
    {
        
        //validate Q5
        var bChecked=false;
         
        for (var k=0; k<=Q5.length-1; k++)
        {
            if (Q5[k].checked==true)
            {
                Q5CheckedValue = Q5[k].value;
                bChecked = true;
                break;
            }
        }
        
        if (!bChecked)
        {
            valobj.innerHTML = "<ul><li>Please answer Question No. 4</li></ul>";
            args.IsValid = false;
            return;
        }
        else if (Q5CheckedValue!="3" && trim(Q6.value)=="")//validate Q6 (if it is not home school)
        {
            valobj.innerHTML = "<ul><li>Please enter Name of the School</li></ul>";
            args.IsValid = false;
            return;
        }
    }
    
}

//*****************************END OF VALIDATION FOR Pittsburgh**********************************


//**************************VALIDATION FOR COLUMBUS QUESTIONNAIRE****************************

////////////////to validate the Step2 (Page 2) for COLUMBUS/////////////////////////
function ValidatePage2Step2_Columbus(sender,args)
{
    var inputobjs = document.getElementsByTagName("input");
    var selectobjs = document.getElementsByTagName("select");
    var Q3_1, Q3_2, Q4, Q5, Q6, bValid = true, Q5CheckedValue;
    var Q5 = new Array();
    var j=0;
    var valobj = document.getElementById(sender.id);
    var Synagogue_0,Synagogue_1,SynagogueOther;
    var SynagogueComboValue;
    var SynRefCode;
    
    for (var i = 0; i< inputobjs.length-1; i++)
    {
        //for Q3_1
        if (inputobjs[i].id.indexOf("RadioBtnQ31")>=0)
            Q3_1 = inputobjs[i];
        //for Q3_2
        if (inputobjs[i].id.indexOf("RadioBtnQ32")>=0)
            Q3_2 = inputobjs[i];
        //for Q5
        if (inputobjs[i].id.indexOf("RadioButtionQ5")>=0)
        {
            Q5[j] = inputobjs[i];
            j=j+1;
        }
        //for Q6
        if (inputobjs[i].id.indexOf("txtCamperSchool")>=0)
            Q6 = inputobjs[i];
            

    }  //end of for loop   

    
    //to get the select objects (ddlgrade) for Q4
    for (var k=0; k<= selectobjs.length-1; k++)
    {
        if (selectobjs[k].id.indexOf("ddlGrade")>=0)
        {
            Q4 = selectobjs[k];
            break;
        }
    }

    
    if (Q3_1.checked==false && Q3_2.checked==false)
    {
        valobj.innerHTML = "<ul><li>Please answer Question No. 1</li></ul>";
        args.IsValid = false;
        return;
    }
    if(Q3_1.checked==true || Q3_2.checked==true)
    {
        var returnVal = ValidateSynagogueAndJCCQuestion(inputobjs,selectobjs,valobj,false);
        if(returnVal == false)
        {
            args.IsValid = false;
            return;
        }
    }

    //validate Q4
    if (bValid && Q4.selectedIndex==0)
    {
        valobj.innerHTML = "<ul><li>Please select a Grade</li></ul>";
        args.IsValid = false;
        return;
    }
    else
    {
        
        //validate Q5
        var bChecked=false;
         
        for (var k=0; k<=Q5.length-1; k++)
        {
            if (Q5[k].checked==true)
            {
                Q5CheckedValue = Q5[k].value;
                bChecked = true;
                break;
            }
        }
        
        if (!bChecked)
        {
            valobj.innerHTML = "<ul><li>Please answer Question No. 4</li></ul>";
            args.IsValid = false;
            return;
        }
        else if (Q5CheckedValue!="3" && trim(Q6.value)=="")//validate Q6 (if it is not home school)
        {
            valobj.innerHTML = "<ul><li>Please enter Name of the School</li></ul>";
            args.IsValid = false;
            return;
        }
    }
    
}

//*****************************END OF VALIDATION FOR Louisville**********************************

//**************************VALIDATION FOR Louisville QUESTIONNAIRE****************************

////////////////to validate the Step2 (Page 2) for Louisville/////////////////////////
function ValidatePage2Step2_Louisville(sender,args)
{
    var inputobjs = document.getElementsByTagName("input");
    var selectobjs = document.getElementsByTagName("select");
    var Q3_1, Q3_2, Q4, Q5, Q6, bValid = true, Q5CheckedValue;
    var Q5 = new Array();
    var j=0;
    var valobj = document.getElementById(sender.id);
    var Synagogue_0,Synagogue_1,SynagogueOther;
    var SynagogueComboValue;
    var SynRefCode;
    
    for (var i = 0; i< inputobjs.length-1; i++)
    {
        //for Q3_1
        if (inputobjs[i].id.indexOf("RadioBtnQ31")>=0)
            Q3_1 = inputobjs[i];
        //for Q3_2
        if (inputobjs[i].id.indexOf("RadioBtnQ32")>=0)
            Q3_2 = inputobjs[i];
        //for Q5
        if (inputobjs[i].id.indexOf("RadioButtionQ5")>=0)
        {
            Q5[j] = inputobjs[i];
            j=j+1;
        }
        //for Q6
        if (inputobjs[i].id.indexOf("txtCamperSchool")>=0)
            Q6 = inputobjs[i];
            

    }  //end of for loop   

    
    //to get the select objects (ddlgrade) for Q4
    for (var k=0; k<= selectobjs.length-1; k++)
    {
        if (selectobjs[k].id.indexOf("ddlGrade")>=0)
        {
            Q4 = selectobjs[k];
            break;
        }
    }

    
    if (Q3_1.checked==false && Q3_2.checked==false)
    {
        valobj.innerHTML = "<ul><li>Please answer Question No. 1</li></ul>";
        args.IsValid = false;
        return;
    }
    if(Q3_1.checked==true || Q3_2.checked==true)
    {
        var returnVal = ValidateSynagogueAndJCCQuestion(inputobjs,selectobjs,valobj,false);
        if(returnVal == false)
        {
            args.IsValid = false;
            return;
        }
    }

    //validate Q4
    if (bValid && Q4.selectedIndex==0)
    {
        valobj.innerHTML = "<ul><li>Please select a Grade</li></ul>";
        args.IsValid = false;
        return;
    }
    else
    {
        
        //validate Q5
        var bChecked=false;
         
        for (var k=0; k<=Q5.length-1; k++)
        {
            if (Q5[k].checked==true)
            {
                Q5CheckedValue = Q5[k].value;
                bChecked = true;
                break;
            }
        }
        
        if (!bChecked)
        {
            valobj.innerHTML = "<ul><li>Please answer Question No. 4</li></ul>";
            args.IsValid = false;
            return;
        }
        else if (Q5CheckedValue!="3" && trim(Q6.value)=="")//validate Q6 (if it is not home school)
        {
            valobj.innerHTML = "<ul><li>Please enter Name of the School</li></ul>";
            args.IsValid = false;
            return;
        }
    }
    
}

//*****************************END OF VALIDATION FOR Louisville**********************************

//*****************************VALIDATION FOR GREENSBORO****************************************

//to validate the Step2 (Page 2) for Greensboro/////////////////////////
function ValidatePage2Step2_GreensBoro(sender,args)
{
    var inputobjs = document.getElementsByTagName("input");
    var selectobjs = document.getElementsByTagName("select");
    var Q3_1, Q3_2, Q4_1, Q4_2, Q5_1, Q5_2, Q6, Q7_1, Q7_2, Q8_1, Q8_2, Q9, Q10_1, Q10_2, Q11;
    Q9 = new Array();
    var j=0;
    var valobj = document.getElementById(sender.id);
    var hdnYearCount;
    for (var i = 0; i< inputobjs.length-1; i++)
    {
        //for Q3_1
        if (inputobjs[i].id.indexOf("RadioBtnQ3_0")>=0)
        {
            Q3_1 = inputobjs[i];
        }
        //for Q3_2
        if (inputobjs[i].id.indexOf("RadioBtnQ3_1")>=0)
        {
            Q3_2 = inputobjs[i];
        }
        
        //for Q4_1
        if (inputobjs[i].id.indexOf("RadioBtnQ4_0")>=0)
        {
            Q4_1 = inputobjs[i];
        }
        
        //for Q4_2
        if (inputobjs[i].id.indexOf("RadioBtnQ4_1")>=0)
        {
            Q4_2 = inputobjs[i];
        }
        
        //for Q5_1
        if (inputobjs[i].id.indexOf("RadioBtnQ5_0")>=0)
        {
            Q5_1 = inputobjs[i];
        }
        
        //for Q5_2
        if (inputobjs[i].id.indexOf("RadioBtnQ5_1")>=0)
        {
            Q5_2 = inputobjs[i];
        }
        
        //for Q7_1
//        if (inputobjs[i].id.indexOf("RadioBtnQ7_0")>=0)
//        {
//            Q7_1 = inputobjs[i];
//        }
//        
//        //for Q7_2
//        if (inputobjs[i].id.indexOf("RadioBtnQ7_1")>=0)
//        {
//            Q7_2 = inputobjs[i];
//        }        
//       
        //for Q9
        if (inputobjs[i].id.indexOf("RadioBtnQ9")>=0)
        {
            Q9[j] = inputobjs[i];
            j=j+1;
        }
        
        //for Q10_2
        if (inputobjs[i].id.indexOf("txtJewishSchool")>=0)
        {
            Q10_2 = inputobjs[i];
        }
        
        //for Q11
        if (inputobjs[i].id.indexOf("txtCamperSchool")>=0)
        {
            Q11 = inputobjs[i];
        }
        
    }  //end of for loop
    
    //to get the <select> objects for Q8 and Q10
    for (var i = 0; i<= selectobjs.length-1; i++)
    {
        //for Q6
         if (selectobjs[i].id.indexOf("ddlGrade")>=0)
        {
            Q6 = selectobjs[i];
        } 
        
        //for Q8
//        if (selectobjs[i].id.indexOf("ddlQ8")>=0)
//        {
//            Q8_1 = selectobjs[i];
//        } 
        
        //for Q10
        if (selectobjs[i].id.indexOf("ddlQ10")>=0)
        {
            Q10_1 = selectobjs[i];
        }
    }

    //validate Q3
    if (Q3_1.checked==false && Q3_2.checked==false)
    {
        valobj.innerHTML = "<ul><li>Please answer Question No. 1</li></ul>";
        args.IsValid=false;
        return;
    }
    else if (Q3_1.checked==true) // if yes is checked
    {
            var returnVal = ValidateSynagogueAndJCCQuestion(inputobjs,selectobjs,valobj,false);
            if(returnVal == false)
            {
                args.IsValid = false;
                return;
            }
//         //for Question 7
//        if (Q7_1.checked==false && Q7_2.checked==false)
//        {
//             valobj.innerHTML = "<li>Please answer Question No. 5</li>";
//             args.IsValid=false;
//             return;
//        }
    }
    else if (Q3_2.checked)  //if "no" is checked
    {
        //to validate for Q4 and Q5
        if (Q4_1.checked==false && Q4_2.checked==false)
        {
             valobj.innerHTML = "<li>Please answer Question No. 2</li>";
             args.IsValid=false;
             return;
        }
        else if (Q4_1.checked)
        {
            if (Q5_1.checked==false && Q5_2.checked==false)
            {
                 valobj.innerHTML = "<li>Please answer Question No. 3</li>";
                 args.IsValid=false;
                 return;
            }
            else if (Q5_1.checked)
            {
                var returnVal = ValidateSynagogueAndJCCQuestion(inputobjs,selectobjs,valobj,false);
                if(returnVal == false)
                {
                    args.IsValid = false;
                    return;
                }
//                //for Question 7
//                if (Q7_1.checked==false && Q7_2.checked==false)
//                {
//                     valobj.innerHTML = "<li>Please answer Question No. 5</li>";
//                     args.IsValid=false;
//                     return;
//                }
            }
            else if(Q5_2.checked)
            {
                var returnVal = ValidateSynagogueAndJCCQuestion(inputobjs,selectobjs,valobj,false);
                if(returnVal == false)
                {
                    args.IsValid = false;
                    return;
                }
            }
        }
        else if(Q4_2.checked)
        {
            var returnVal = ValidateSynagogueAndJCCQuestion(inputobjs,selectobjs,valobj,false);
            if(returnVal == false)
            {
                args.IsValid = false;
                return;
            }
        }

    }
    
   
      
    //for Question 8 ' trim(Q8_2.value)=="" && Q8_1.selectedIndex==0 && Q7_1.checked==true
//    if (Q8_1.selectedIndex==0 && Q7_1.checked==true)
//    {
//         valobj.innerHTML = "<li>Please answer Question No. 6</li>";
//         args.IsValid=false;
//         return;
//    }
    
    //validate Q6
    if (Q6.selectedIndex==0)
    {
        valobj.innerHTML = "<li>Please select the Grade</li>";
        args.IsValid=false;
        return;
    }
   
    //for Question 9
    var bQ9Checked = false;
    for (var j =0 ; j<= Q9.length-1; j++)
    {
        if (Q9[j].checked && Q9[j].value==4)
        {
            bQ9Checked = true;
            if (trim(Q10_2.value)=="" && Q10_1.selectedIndex==0)
            {
                 valobj.innerHTML = "<li>Please answer Question No. 7</li>";
                 args.IsValid=false;
                 return;
            }
            else if (trim(Q10_2.value)=="" && Q10_1.selectedIndex==3)  //others is selected
            {
                 valobj.innerHTML = "<li>Please type in the Jewsish Day School</li>";
                 Q10_2.focus();
                 args.IsValid=false;
                 return;
            }
            else if (trim(Q10_2.value)!="")
            {
                Q10_1.selectedIndex=3;
            }
        }
        else if (Q9[j].checked) //Q9 has been answered
        {
            bQ9Checked=true;
            //for Question 11
            //Changed by Ram (11/1/2009)
            //if (trim(Q11.value)=="" && Q9[j].value!=3)
            if (trim(Q11.value)=="")
            {
                valobj.innerHTML = "<li>Please enter Name of the School</li>";
                args.IsValid=false;
                return;
            }   
        }
    }
    
    //if Q9 is not answered
    if (!bQ9Checked)
    {
         valobj.innerHTML = "<li>Please answer Question No. 6</li>";
         args.IsValid=false;
         return;
    }    
 
  
    args.IsValid = true;
    return;
}


//to validate the Step2 (Page 3) of Greenboro Questionaire/////////////////////////
function VaildatePage3Step2_Greensboro(sender,args)
{
    var Q12, Q13_State, Q13_Camp, Q14_CampSession, Q15_StartDate, Q15_EndDate;
    Q12 = new Array();
    var k=0;
    var bRadioOption1 = false;
    var bRadioOptionRest = false;
    var inputobjs = document.getElementsByTagName("input");
    var selectobjs = document.getElementsByTagName("select");
    var valObj = document.getElementById(sender.id);
    var bValid=false;
    var strErrorMsg="";
    
    var startDate = document.getElementById("ctl00_hdnCampSessionStartDate");
    var endDate = document.getElementById("ctl00_hdnCampSessionEndDate");
    var campSeasonErrorMessage = document.getElementById("ctl00_hdncampSeasonErrorMessage");

    for (var i = 0; i<inputobjs.length-1; i++)
    {
        if (inputobjs[i].id.indexOf("RadioBtnQ12") >=0)
        {
            Q12[k] = inputobjs[i];
            k=k+1;
        }
        else if (inputobjs[i].id.indexOf("txtCampSession")>=0)
            Q14_CampSession = inputobjs[i];  
        else if (inputobjs[i].id.indexOf("txtStartDate")>=0)
            Q15_StartDate = inputobjs[i];
        else if (inputobjs[i].id.indexOf("txtEndDate")>=0)
            Q15_EndDate = inputobjs[i];
    } //end of for loop
    
    //to select the dropdown objs
    for (var j=0; j<=selectobjs.length-1; j++)
    {
        if (selectobjs[j].id.indexOf("ddlState")>=0)
            Q13_State = selectobjs[j];
        else if (selectobjs[j].id.indexOf("ddlCamp")>=0)
            Q13_Camp = selectobjs[j];
    }
    
    //for Q10
    for (var k = 0; k < Q12.length; k++)
    {
        if (Q12[k].id.indexOf("RadioBtnQ12_0")>=0)  //then option 1 is checked
        {
            if (Q12[k].checked)
            {
                bRadioOption1 = true;
                break;
            }
        }
        else if (Q12[k].checked)
        {
            bRadioOptionRest =true;
            break;
        }
            
    }
   
    //validation for Question 7
    if (bRadioOption1)
        bValid=true;
    
    else if (bRadioOptionRest)
    {
        Q14_CampSession.value = trim(Q14_CampSession.value);
        Q15_StartDate.value = trim(Q15_StartDate.value);
        Q15_EndDate.value = trim(Q15_EndDate.value);
        //validation for the rest of the questions
        //for Question 13 
        if (Q13_Camp.selectedIndex==0)
        {
            strErrorMsg="<li>Please select a Camp</li>";
            bValid = false;
        }
        else if (Q14_CampSession.value=="") //for Question 14
        {
            strErrorMsg="<li>Please enter a Camp Session</li>";
            bValid = false;
        }
        else if (Q15_StartDate.value=="" || Q15_EndDate.value=="") //for Question 15
        {
            strErrorMsg="<li>Please enter Start Date and End Date in the mm/dd/yyyy format, or select the dates by clicking the calendar icons</li>";
            bValid = false;
        }
        else if (!ValidateDate(Q15_StartDate.value))
        {
            strErrorMsg="<li>Please enter a Valid Start Date in the mm/dd/yyyy format, or select the date by clicking the calendar icon</li>";
            bValid = false;
        }
        else if (!ValidateDate(Q15_EndDate.value))
        {
            strErrorMsg="<li>Please enter a Valid End Date in the mm/dd/yyyy format, or select the date by clicking the calendar icon</li>";
            bValid = false;
        }
        else if (!CompareDates(Q15_StartDate.value,Q15_EndDate.value))
        {
            strErrorMsg="<li>Start Date should be less than the End Date</li>";
            bValid = false;
        }
        //Added by Ram (10/15/2009) related to allow "May, Jun, Jul, Aug, Sep" as session months
        
        else if (!CompareDates(startDate.value,Q10_StartDate.value))
        {            
            strErrorMsg="<li>"+campSeasonErrorMessage.value+"</li>";
            bValid = false;
        }
        else if (!CompareDates(Q10_StartDate.value,endDate.value))
        {   
            strErrorMsg="<li>"+campSeasonErrorMessage.value+"</li>";
            bValid = false;
        }
        else if (!CompareDates(startDate.value,Q10_EndDate.value))
        {   
            strErrorMsg="<li>"+campSeasonErrorMessage.value+"</li>";
            bValid = false;
        }
        else if (!CompareDates(Q10_EndDate.value,endDate.value))
        {   
            strErrorMsg="<li>"+campSeasonErrorMessage.value+"</li>";
            bValid = false;
        }
        else
            bValid = true;
    }//end of else if
    else  //Question 12 is not answered
    {
        strErrorMsg="<li>Please answer Question No. 12</li>";
        bValid = false;
    }
    
    valObj.innerHTML = strErrorMsg;
    args.IsValid = bValid;
    return;
}


//*****************************END OF VALIDATION FOR GREENSBORO****************************************



//****************************VALIDATION FOR JWEST*****************************************************

//to validate the Step2 (Page 2) for Jwest/////////////////////////
function ValidatePage2Step2_JWest(sender,args)
{
    var inputobjs = document.getElementsByTagName("input");
    var selectobjs = document.getElementsByTagName("select");
    var Q3_1, Q3_2, Q4_1, Q4_2, Q4_3, Q5_1, Q5_2, strYear, strNoofDays;
    Q9 = new Array();
    var j=0;
    var valobj = document.getElementById(sender.id);
    var hdnYearCount;
    for (var i = 0; i< inputobjs.length-1; i++)
    {
        //for Q3_1
        if (inputobjs[i].id.indexOf("RadioBtnQ3_0")>=0)
        {
            Q3_1 = inputobjs[i];
        }
        //for Q3_2
        if (inputobjs[i].id.indexOf("RadioBtnQ3_1")>=0)
        {
            Q3_2 = inputobjs[i];
        }
        
        //for Q4_1
        if (inputobjs[i].id.indexOf("RadioBtnQ4_0")>=0)
        {
            Q4_1 = inputobjs[i];
        }
        
        //for Q4_2
        if (inputobjs[i].id.indexOf("RadioBtnQ4_1")>=0)
        {
            Q4_2 = inputobjs[i];
        }
        
         //for Q4_3
        if (inputobjs[i].id.indexOf("RadioBtnQ4_2")>=0)
        {
            Q4_3 = inputobjs[i];
        }       

    }  //end of for loop
    
    //validate Q3
    if (Q3_1.checked==false && Q3_2.checked==false)
    {
        valobj.innerHTML = "<ul><li>Please answer Question No. 1</li></ul>";
        args.IsValid=false;
        return;
    }
    else if (Q3_2.checked)  //if "no" is checked
    {
        //to validate for Q4 and Q5
        if (Q4_1.checked==false && Q4_2.checked==false && Q4_3.checked==false)
        {
             valobj.innerHTML = "<li>Please answer Question No. 2</li>";
             args.IsValid=false;
             return;
        }
    }   
  
    args.IsValid = true;
    return;
}

//to validate the Step2 (Page 3) of Jwest Questionaire/////////////////////////
function VaildatePage3Step2_JWest(sender,args)
{
    var Q6, Q7, Q8, Q9_StartDate, Q9_EndDate;;
    Q6 = new Array();
    var k=0;
    var bRadioOption1 = false;
    var bOptionRest = false;
    var inputobjs = document.getElementsByTagName("input");
    var selectobjs = document.getElementsByTagName("select");
    var valObj = document.getElementById(sender.id);
    var bValid=false;
    var strErrorMsg="";
    var dt = new Date();
    var currentDt = dt.getMonth()+ 1 + "/" + dt.getDate() + "/" + dt.getYear();    
    var startDate = document.getElementById("ctl00_hdnCampSessionStartDate");
    var endDate = document.getElementById("ctl00_hdnCampSessionEndDate");
    var campSeasonErrorMessage = document.getElementById("ctl00_hdncampSeasonErrorMessage");

    for (var i = 0; i<inputobjs.length-1; i++)
    {
        if (inputobjs[i].id.indexOf("RadioBtnQ6") >=0)
        {
            Q6[k] = inputobjs[i];
            k=k+1;
        }
        else if (inputobjs[i].id.indexOf("txtCampSession")>=0)
            Q8 = inputobjs[i];  
        else if (inputobjs[i].id.indexOf("txtStartDate")>=0)
            Q9_StartDate = inputobjs[i];
        else if (inputobjs[i].id.indexOf("txtEndDate")>=0)
            Q9_EndDate = inputobjs[i];
    } //end of for loop
    
    //to select the dropdown objs
    for (var j=0; j<=selectobjs.length-1; j++)
    {
//        if (selectobjs[j].id.indexOf("ddlCampSession")>=0)
//            Q8 = selectobjs[j];
//        else 
        if (selectobjs[j].id.indexOf("ddlCamp")>=0)
            Q7 = selectobjs[j];
    }
    
    //for Q6
    for (var k = 0; k <=Q6.length-1; k++)
    {
        if (Q6[k].id.indexOf("RadioBtnQ6_0")>=0)  //then option 1 is checked
        {
            if (Q6[k].checked)
            {
                bRadioOption1 = true;
                break;
            }
        }
        else if (Q6[k].checked)
        {
            bOptionRest =true;   //one among the other three options is selected
            break;
        }
            
    }
   
    //validation for Question 6
//    if (bRadioOption1)
//        bValid=true;
    
//    else if (bOptionRest) //one among other 3 option is selected

    if(bRadioOption1)
    {
        //validation for the rest of the questions
        Q8.value = trim(Q8.value);
        Q9_StartDate.value = trim(Q9_StartDate.value);
        Q9_EndDate.value = trim(Q9_EndDate.value);
        //for Question 7 
        if (Q7.selectedIndex==0)
        {
            strErrorMsg="<li>Please select a Camp</li>";
            bValid = false;
        }
         else if (Q8.value=="") //for Question 8
        {
            strErrorMsg="<li>Please enter the Camp Session</li>";
            bValid = false;
        }
        else if (Q9_StartDate.value=="" || Q9_EndDate.value=="") //for Question 9
        {
            strErrorMsg="<li>Please enter dates in the mm/dd/yyyy format, or select the dates by clicking the calendar icons</li>";
            bValid = false;
        }
        else if (!ValidateDate(Q9_StartDate.value))
        {
            strErrorMsg="<li>Please enter a Valid Start Date in the mm/dd/yyyy format, or select the date by clicking the calendar icon</li>";
            bValid = false;
        }
        else if (!ValidateDate(Q9_EndDate.value))
        {
            strErrorMsg="<li>Please enter a Valid End Date in the mm/dd/yyyy format, or select the date by clicking the calendar icon</li>";
            bValid = false;
        }
        else if (!CompareDates(Q9_StartDate.value,Q9_EndDate.value))
        {
            strErrorMsg="<li>Start Date should be less than the End Date</li>";
            bValid = false;
        }
            //Added by Ram (10/15/2009) related to allow "May, Jun, Jul, Aug, Sep" as session months
        
        else if (!CompareDates(startDate.value,Q9_StartDate.value))
        {            
            strErrorMsg="<li>"+campSeasonErrorMessage.value+"</li>";
            bValid = false;
        }
        else if (!CompareDates(Q9_StartDate.value,endDate.value))
        {   
            strErrorMsg="<li>"+campSeasonErrorMessage.value+"</li>";
            bValid = false;
        }
        else if (!CompareDates(startDate.value,Q9_EndDate.value))
        {   
            strErrorMsg="<li>"+campSeasonErrorMessage.value+"</li>";
            bValid = false;
        }
        else if (!CompareDates(Q9_EndDate.value,endDate.value))
        {   
            strErrorMsg="<li>"+campSeasonErrorMessage.value+"</li>";
            bValid = false;
        }
        else
            bValid = true;
//        else if (Q8.selectedIndex==0) //for Question 8
//        {
//            strErrorMsg="<li>Please select a Camp Session</li>";
//            bValid = false;
//        }
//        else
//            bValid = true;
    }//end of else if
    else  //Question 6 is not answered
    {
        strErrorMsg="<li>Please answer the Camp registration Question.</li>";
        bValid = false;
    }   
 
    valObj.innerHTML = strErrorMsg;
    args.IsValid = bValid;
    return;
}


//****************************************END OF VALIDATION FOR JWEST**************************************

//****************************************VALIDATION FOR ORANGE COUNTY**************************************

//to validate the Step2 (Page 2) for Orange/////////////////////////
function ValidatePage2Step2_Orange(sender,args)
{
    var inputobjs = document.getElementsByTagName("input");
    var selectobjs = document.getElementsByTagName("select");
    var Q3_1, Q3_2, Q4_1, Q4_2, Q5_1, Q5_2;
    
    var j=0;
    var valobj = document.getElementById(sender.id);
    for (var i = 0; i< inputobjs.length-1; i++)
    {
        //for Q3_1
        if (inputobjs[i].id.indexOf("RadioBtnQ3_0")>=0)
        {
            Q3_1 = inputobjs[i];
        }
        //for Q3_2
        if (inputobjs[i].id.indexOf("RadioBtnQ3_1")>=0)
        {
            Q3_2 = inputobjs[i];
        }
        
        //for Q4_1
        if (inputobjs[i].id.indexOf("RadioBtnQ4_0")>=0)
        {
            Q4_1 = inputobjs[i];
        }
        
        //for Q4_2
        if (inputobjs[i].id.indexOf("RadioBtnQ4_1")>=0)
        {
            Q4_2 = inputobjs[i];
        }
        
        //for Q5_1
        if (inputobjs[i].id.indexOf("RadioBtnQ5_0")>=0)
        {
            Q5_1 = inputobjs[i];
        }
        
        //for Q5_2
        if (inputobjs[i].id.indexOf("RadioBtnQ5_1")>=0)
        {
            Q5_2 = inputobjs[i];
        }
    }  //end of for loop
    
    //validate Q3
    if (Q3_1.checked==false && Q3_2.checked==false)
    {
        valobj.innerHTML = "<ul><li>Please answer Question No. 1</li></ul>";
        args.IsValid=false;
        return;
    }
    else if (Q3_2.checked)  //if "no" is checked
    {
        //to validate for Q4 and Q5
        if (Q4_1.checked==false && Q4_2.checked==false)
        {
             valobj.innerHTML = "<li>Please answer Question No. 2</li>";
             args.IsValid=false;
             return;
        }
        else if (Q4_1.checked)
        {
            if (Q5_1.checked==false && Q5_2.checked==false)
            {
                 valobj.innerHTML = "<li>Please answer Question No. 3</li>";
                 args.IsValid=false;
                 return;
            }
        }
    } 

    args.IsValid = true;
    return;
}



//to validate the Step2 (Page 3) of Orange Questionaire/////////////////////////
function VaildatePage3Step2_Orange(sender,args)
{
    var Q6, Q7, Q8, Q9_StartDate, Q9_EndDate;
    Q6 = new Array();
    var k=0;
    var bRadioOption1 = false;
    var bOptionRest = false;
    var inputobjs = document.getElementsByTagName("input");
    var selectobjs = document.getElementsByTagName("select");
    var valObj = document.getElementById(sender.id);
    var dt = new Date();
    var currentDt = dt.getMonth()+ 1 + "/" + dt.getDate() + "/" + dt.getYear();    
    var bValid=false;
    var strErrorMsg="";
    var startDate = document.getElementById("ctl00_hdnCampSessionStartDate");
    var endDate = document.getElementById("ctl00_hdnCampSessionEndDate");
    var campSeasonErrorMessage = document.getElementById("ctl00_hdncampSeasonErrorMessage");

    for (var i = 0; i<inputobjs.length-1; i++)
    {
        if (inputobjs[i].id.indexOf("RadioBtnQ6") >=0)
        {
            Q6[k] = inputobjs[i];
            k=k+1;
        }
        else if (inputobjs[i].id.indexOf("txtCampSession")>=0)
            Q8 = inputobjs[i];  
        else if (inputobjs[i].id.indexOf("txtStartDate")>=0)
            Q9_StartDate = inputobjs[i];
        else if (inputobjs[i].id.indexOf("txtEndDate")>=0)
            Q9_EndDate = inputobjs[i];
    } //end of for loop
    
    //to select the dropdown objs
    for (var j=0; j<=selectobjs.length-1; j++)
    {
        if (selectobjs[j].id.indexOf("ddlCamp")>=0)
            Q7 = selectobjs[j];
    }
    
    //for Q6
    for (var k = 0; k <=Q6.length-1; k++)
    {
        if (Q6[k].id.indexOf("RadioBtnQ6_0")>=0)  //then option 1 is checked
        {
            if (Q6[k].checked)
            {
                bRadioOption1 = true;
                break;
            }
        }
        else if (Q6[k].checked)
        {
            bOptionRest =true;   //one among the other three options is selected
            break;
        }
            
    }
   
    //validation for Question 6
    if (bRadioOption1)
        bValid=true;
    
    else if (bOptionRest) //one among other 3 option is selected
    {
        //validation for the rest of the questions
        Q8.value = trim(Q8.value);
        Q9_StartDate.value = trim(Q9_StartDate.value);
        Q9_EndDate.value = trim(Q9_EndDate.value);
        //for Question 7 
        if (Q7.selectedIndex==0)
        {
            strErrorMsg="<li>Please select a Camp</li>";
            bValid = false;
        }
        else if (Q8.value=="") //for Question 8
        {
            strErrorMsg="<li>Please enter the Camp Session</li>";
            bValid = false;
        }
        else if (Q9_StartDate.value=="" || Q9_EndDate.value=="") //for Question 9
        {
            strErrorMsg="<li>Please enter dates in the mm/dd/yyyy format, or select the dates by clicking the calendar icons</li>";
            bValid = false;
        }
//        else if (!CompareDates(currentDt,Q9_StartDate.value))
//        {
//            strErrorMsg="<li>Start date can not be less than today's date</li>";
//            bValid = false;
//        }
        else if (!ValidateDate(Q9_StartDate.value))
        {
            strErrorMsg="<li>Please enter a Valid Start Date in the mm/dd/yyyy format, or select the date by clicking the calendar icon</li>";
            bValid = false;
        }
        else if (!ValidateDate(Q9_EndDate.value))
        {
            strErrorMsg="<li>Please enter a Valid End Date in the mm/dd/yyyy format, or select the date by clicking the calendar icon</li>";
            bValid = false;
        }
        else if (!CompareDates(Q9_StartDate.value,Q9_EndDate.value))
        {
            strErrorMsg="<li>Start Date should be less than the End Date</li>";
            bValid = false;
        }
            //Added by Ram (10/15/2009) related to allow "May, Jun, Jul, Aug, Sep" as session months
        
        else if (!CompareDates(startDate.value,Q10_StartDate.value))
        {            
            strErrorMsg="<li>"+campSeasonErrorMessage.value+"</li>";
            bValid = false;
        }
        else if (!CompareDates(Q10_StartDate.value,endDate.value))
        {   
            strErrorMsg="<li>"+campSeasonErrorMessage.value+"</li>";
            bValid = false;
        }
        else if (!CompareDates(startDate.value,Q10_EndDate.value))
        {   
            strErrorMsg="<li>"+campSeasonErrorMessage.value+"</li>";
            bValid = false;
        }
        else if (!CompareDates(Q10_EndDate.value,endDate.value))
        {   
            strErrorMsg="<li>"+campSeasonErrorMessage.value+"</li>";
            bValid = false;
        }
        else
            bValid = true;
    }//end of else if
    else  //Question 6 is not answered
    {
        strErrorMsg="<li>Please answer Question No. 6</li>";
        bValid = false;
    } 

    valObj.innerHTML = strErrorMsg;
    args.IsValid = bValid;
    return;
}




//******************************VALIDATION FOR LA CIP************************************

//to validate the Step2 (Page 2) for LA CIP/////////////////////////
function ValidatePage2Step2_LACIP(sender,args)
{
    var inputobjs = document.getElementsByTagName("input");
    var selectobjs = document.getElementsByTagName("select");
    var Q3_1, Q3_2, Q4_1, Q4_2, Q5_1, Q5_2, Q6_1, Q6_2;
    
    var j=0;
    var valobj = document.getElementById(sender.id);
    for (var i = 0; i< inputobjs.length-1; i++)
    {
        //for Q3_1
        if (inputobjs[i].id.indexOf("RadioBtnQ3_0")>=0)
            Q3_1 = inputobjs[i];
        //for Q3_2
        if (inputobjs[i].id.indexOf("RadioBtnQ3_1")>=0)
            Q3_2 = inputobjs[i];
        //for Q4_1
        if (inputobjs[i].id.indexOf("RadioBtnQ4_0")>=0)
            Q4_1 = inputobjs[i];
        //for Q4_2
        if (inputobjs[i].id.indexOf("RadioBtnQ4_1")>=0)
            Q4_2 = inputobjs[i];
        //for Q5_1
        if (inputobjs[i].id.indexOf("RadioBtnQ5_0")>=0)
            Q5_1 = inputobjs[i];
        //for Q5_2
        if (inputobjs[i].id.indexOf("RadioBtnQ5_1")>=0)
            Q5_2 = inputobjs[i];
        //for Q6_1
//        if (inputobjs[i].id.indexOf("RadioBtnQ6_0")>=0)
//            Q6_1 = inputobjs[i];
//        //for Q6_2
//        if (inputobjs[i].id.indexOf("RadioBtnQ6_1")>=0)
//            Q6_2 = inputobjs[i];
    }  //end of for loop
    
    //validate Q3
    if (Q3_1.checked==false && Q3_2.checked==false)
    {
        valobj.innerHTML = "<ul><li>Please answer Question No. 1</li></ul>";
        args.IsValid=false;
        return;
    }
    else if (Q3_2.checked)  //if "no" is checked
    {
        //to validate for Q4 and Q5
//        if (Q4_1.checked==false && Q4_2.checked==false)
//        {
//             valobj.innerHTML = "<li>Please answer Question No. 2</li>";
//             args.IsValid=false;
//             return;
//        }
//        else if (Q4_1.checked)
//        {
//            if (Q5_1.checked==false && Q5_2.checked==false)
//            {
//                 valobj.innerHTML = "<li>Please answer Question No. 3</li>";
//                 args.IsValid=false;
//                 return;
//            }
//        }
    }
//    else if (Q3_1.checked) // if "yes" is selected
//    {
//        if (Q6_1.checked==false && Q6_2.checked==false) //Question 6 is not answered
//        {
//            valobj.innerHTML = "<li>Please answer Question No. 5</li>";
//            args.IsValid=false;
//            return;
//        }
//    }  

    args.IsValid = true;
    return;
}


//to validate the Step2 (Page 3) of LA CIP Questionaire/////////////////////////
function VaildatePage3Step2_LACIP(sender,args)
{
    var Q7, Q8, Q9, Q10_StartDate, Q10_EndDate, chkAcknowledge;
    Q7 = new Array();
    var k=0;
    var bRadioOption1 = false;
    var bOptionRest = false;
    var inputobjs = document.getElementsByTagName("input");
    var selectobjs = document.getElementsByTagName("select");
    var valObj = document.getElementById(sender.id);
    var bValid=false;
    var strErrorMsg="";
    
    var dt = new Date();
    var currentDt = dt.getMonth()+ 1 + "/" + dt.getDate() + "/" + dt.getYear();   
    var startDate = document.getElementById("ctl00_hdnCampSessionStartDate");
    var endDate = document.getElementById("ctl00_hdnCampSessionEndDate");
    var campSeasonErrorMessage = document.getElementById("ctl00_hdncampSeasonErrorMessage");
    
    for (var i = 0; i<inputobjs.length-1; i++)
    {
        if (inputobjs[i].id.indexOf("RadioBtnQ7") >=0)
        {
            Q7[k] = inputobjs[i];
            k=k+1;
        }
        else if (inputobjs[i].id.indexOf("txtCampSession")>=0)
            Q9 = inputobjs[i];  
        else if (inputobjs[i].id.indexOf("txtStartDate")>=0)
            Q10_StartDate = inputobjs[i];
        else if (inputobjs[i].id.indexOf("txtEndDate")>=0)
            Q10_EndDate = inputobjs[i];
        else if (inputobjs[i].id.indexOf("chkAcknowledgement")>=0)
            chkAcknowledge = inputobjs[i];
    } //end of for loop
    
    //to select the dropdown objs
    for (var j=0; j<=selectobjs.length-1; j++)
    {   
//        if (selectobjs[j].id.indexOf("ddlCampSession")>=0)
//            Q9 = selectobjs[j];
//        else 
        if (selectobjs[j].id.indexOf("ddlCamp")>=0)
            Q8 = selectobjs[j];        
    }
    
    
    //for Q7
    for (var k = 0; k <=Q7.length-1; k++)
    {
        if (Q7[k].id.indexOf("RadioBtnQ7_0")>=0)  //then option 1 is checked
        {
            if (Q7[k].checked)
            {
                bRadioOption1 = true;
                break;
            }
        }
        else if (Q7[k].checked)
        {
            bOptionRest =true;   //one among the other three options is selected
            break;
        }
    }
   
    //validation for Question 6
//    if (bRadioOption1)
//        bValid=true;
//    
//    else if (bOptionRest) //one among other 3 option is selected
    if (bRadioOption1)
    {
        if(document.getElementById("ctl00_Content_lblMsg") !=null)
           document.getElementById("ctl00_Content_lblMsg").style.display = 'none'; 
        //validation for the rest of the questions
        Q9.value = trim(Q9.value);
        Q10_StartDate.value = trim(Q10_StartDate.value);
        Q10_EndDate.value = trim(Q10_EndDate.value);
        
        
        //for Question 8 
        if (Q8.selectedIndex==0)
        {
            strErrorMsg="<li>Please select a Camp</li>";
            bValid = false;
        }
        else if (Q9.value=="") //for Question 9
        {
            strErrorMsg="<li>Please enter the Camp Session</li>";
            bValid = false;
        }
//        else if (Q9.selectedIndex==0) //for Question 9
//        {
//            strErrorMsg="<li>Please select a Camp Session</li>";
//            bValid = false;
//        }
        else if (Q10_StartDate.value=="" || Q10_EndDate.value=="") //for Question 10
        {
            strErrorMsg="<li>Please enter dates in the mm/dd/yyyy format, or select the dates by clicking the calendar icons.</li>";
            bValid = false;
        }
        else if (!ValidateDate(Q10_StartDate.value))
        {
            strErrorMsg="<li>Please enter a Valid Start Date in the mm/dd/yyyy format, or select the date by clicking the calendar icon.</li>";
            bValid = false;
        }
        else if (!ValidateDate(Q10_EndDate.value))
        {
            strErrorMsg="<li>Please enter a Valid End Date in the mm/dd/yyyy format, or select the date by clicking the calendar icon.</li>";
            bValid = false;
        }
        else if (!CompareDates(Q10_StartDate.value,Q10_EndDate.value))
        {
        
            strErrorMsg="<li>Start Date should be less than the End Date</li>";
            bValid = false;
        }
        //Added by Ram (10/15/2009) related to allow "May, Jun, Jul, Aug, Sep" as session months
        
        else if (!CompareDates(startDate.value,Q10_StartDate.value))
        {            
            strErrorMsg="<li>"+campSeasonErrorMessage.value+"</li>";
            bValid = false;
        }
        else if (!CompareDates(Q10_StartDate.value,endDate.value))
        {   
            strErrorMsg="<li>"+campSeasonErrorMessage.value+"</li>";
            bValid = false;
        }
        else if (!CompareDates(startDate.value,Q10_EndDate.value))
        {   
            strErrorMsg="<li>"+campSeasonErrorMessage.value+"</li>";
            bValid = false;
        }
        else if (!CompareDates(Q10_EndDate.value,endDate.value))
        {   
            strErrorMsg="<li>"+campSeasonErrorMessage.value+"</li>";
            bValid = false;
        }

//        else if (!CompareDates(currentDt,Q10_StartDate.value))
//        {
//            strErrorMsg="<li>Start date can not be less than today's date</li>";
//            bValid = false;
//        }
        else
            bValid = true;
    }//end of else if
    else  //Question 7 is not answered
    {
        strErrorMsg="<li>Please answer the Camp registration Question.</li>";
        bValid = false;
    }
    
    //to validate whether the release form has been acknowledged
    if (bValid && chkAcknowledge.checked==false)
    {
        strErrorMsg="<li>Please review the release form and Acknowledge</li>";
        bValid = false;
    } 

    valObj.innerHTML = strErrorMsg;
    args.IsValid = bValid;
    return;
}


//******************************END OF VALIDATION FOR LA CIP************************************


//********************************VALIDATION FOR JWEST LA****************************************

//to validate the Step2 (Page 2) for Jwest LA /////////////////////////
function ValidatePage2Step2_JWestLA(sender,args)
{
    var inputobjs = document.getElementsByTagName("input");
    var selectobjs = document.getElementsByTagName("select");
    var Q3_1, Q3_2, Q4_1, Q4_2, Q4_3, Q5_1, Q5_2, strYear, strNoofDays;
    Q9 = new Array();
    var j=0;
    var valobj = document.getElementById(sender.id);
    var hdnYearCount;
    for (var i = 0; i< inputobjs.length-1; i++)
    {
        //for Q3_1
        if (inputobjs[i].id.indexOf("RadioBtnQ3_0")>=0)
        {
            Q3_1 = inputobjs[i];
        }
        //for Q3_2
        if (inputobjs[i].id.indexOf("RadioBtnQ3_1")>=0)
        {
            Q3_2 = inputobjs[i];
        }
        
        //for Q4_1
        if (inputobjs[i].id.indexOf("RadioBtnQ4_0")>=0)
        {
            Q4_1 = inputobjs[i];
        }
        
        //for Q4_2
        if (inputobjs[i].id.indexOf("RadioBtnQ4_1")>=0)
        {
            Q4_2 = inputobjs[i];
        }
        
         //for Q4_3
        if (inputobjs[i].id.indexOf("RadioBtnQ4_2")>=0)
        {
            Q4_3 = inputobjs[i];
        }      
    }  //end of for loop
    
    //validate Q3
    if (Q3_1.checked==false && Q3_2.checked==false)
    {
        valobj.innerHTML = "<ul><li>Please answer Question No. 1</li></ul>";
        args.IsValid=false;
        return;
    }
    else if (Q3_2.checked)  //if "no" is checked
    {
        //to validate for Q4 and Q5
        if (Q4_1.checked==false && Q4_2.checked==false && Q4_3.checked==false)
        {
             valobj.innerHTML = "<li>Please answer Question No. 2</li>";
             args.IsValid=false;
             return;
        }
    }  

    args.IsValid = true;
    return;
}



//to validate the Step2 (Page 3) of Jwest LA Questionaire/////////////////////////
function VaildatePage3Step2_JWestLA(sender,args)
{
    var Q6, Q7, Q8;
    Q6 = new Array();
    var k=0;
    var bRadioOption1 = false;
    var bOptionRest = false;
    var inputobjs = document.getElementsByTagName("input");
    var selectobjs = document.getElementsByTagName("select");
    var valObj = document.getElementById(sender.id);
    var bValid=false;
    var strErrorMsg="";
    for (var i = 0; i<inputobjs.length-1; i++)
    {
        if (inputobjs[i].id.indexOf("RadioBtnQ6") >=0)
        {
            Q6[k] = inputobjs[i];
            k=k+1;
        }
    } //end of for loop
    
    //to select the dropdown objs
    for (var j=0; j<=selectobjs.length-1; j++)
    {
        if (selectobjs[j].id.indexOf("ddlCampSession")>=0)
            Q8 = selectobjs[j];
        else if (selectobjs[j].id.indexOf("ddlCamp")>=0)
            Q7 = selectobjs[j];
    }
    
    //for Q6
    for (var k = 0; k <=Q6.length-1; k++)
    {
        if (Q6[k].id.indexOf("RadioBtnQ6_0")>=0)  //then option 1 is checked
        {
            if (Q6[k].checked)
            {
                bRadioOption1 = true;
                break;
            }
        }
        else if (Q6[k].checked)
        {
            bOptionRest =true;   //one among the other three options is selected
            break;
        }
            
    }
   
    //validation for Question 6
    if (bRadioOption1)
        bValid=true;
    
    else if (bOptionRest) //one among other 3 option is selected
    {
        //validation for the rest of the questions
        //for Question 7 
        if (Q7.selectedIndex==0)
        {
            strErrorMsg="<li>Please select a Camp</li>";
            bValid = false;
        }
        else if (Q8.selectedIndex==0) //for Question 8
        {
            strErrorMsg="<li>Please select a Camp Session</li>";
            bValid = false;
        }
        else
            bValid = true;
    }//end of else if
    else  //Question 6 is not answered
    {
        strErrorMsg="<li>Please answer Question No. 4</li>";
        bValid = false;
    }  
    
    valObj.innerHTML = strErrorMsg;
    args.IsValid = bValid;
    return;
}
function IsNumeric(sText)
{
   var ValidChars = "0123456789.";
   var IsNumber=true;
   var Char;

 
   for (i = 0; i < sText.length && IsNumber == true; i++) 
      { 
      Char = sText.charAt(i); 
      if (ValidChars.indexOf(Char) == -1) 
         {
         IsNumber = false;
         }
      }
   return IsNumber;
   
}

//**************************************END OF VALIDATION FOR JWEST LA**************************
//to validate the Step2 (Page 3) of URJ Questionaire/////////////////////////
function VaildatePage3Step2_URJ(sender,args)
{
    var Q7_1, Q7_2, Q7_3, Q7_4, Q8_State, Q8_Camp, Q9_CampSession, Q10_StartDate, Q10_EndDate;
    var inputobjs = document.getElementsByTagName("input");
    var selectobjs = document.getElementsByTagName("select");
    var valObj = document.getElementById(sender.id);
    var dt = new Date();
    var currentDt = dt.getMonth()+ 1 + "/" + dt.getDate() + "/" + dt.getYear();
    var bValid=false;
    var strErrorMsg="";
    
    var startDate = document.getElementById("ctl00_hdnCampSessionStartDate");
    var endDate = document.getElementById("ctl00_hdnCampSessionEndDate");
    var campSeasonErrorMessage = document.getElementById("ctl00_hdncampSeasonErrorMessage");

    for (var i = 0; i<inputobjs.length-1; i++)
    {
        if (inputobjs[i].id.indexOf("RadioButtonQ7Option1")>=0)
            Q7_1 = inputobjs[i];
        else if (inputobjs[i].id.indexOf("RadioButtonQ7Option2")>=0)
            Q7_2 = inputobjs[i];
//        else if (inputobjs[i].id.indexOf("RadioButtonQ7Option3")>=0)
//            Q7_3 = inputobjs[i];
//        else if (inputobjs[i].id.indexOf("RadioButtonQ7Option4")>=0)
//            Q7_4 = inputobjs[i];
        else if (inputobjs[i].id.indexOf("RadioButtonQ7Option2")>=0)
            Q7_2 = inputobjs[i];
        else if (inputobjs[i].id.indexOf("txtCampSession")>=0)
            Q9_CampSession = inputobjs[i];  
        else if (inputobjs[i].id.indexOf("txtStartDate")>=0)
            Q10_StartDate = inputobjs[i];
        else if (inputobjs[i].id.indexOf("txtEndDate")>=0)
            Q10_EndDate = inputobjs[i];
    } //end of for loop
    
    //to select the dropdown objs
    for (var j=0; j<=selectobjs.length-1; j++)
    {
        if (selectobjs[j].id.indexOf("ddlState")>=0)
            Q8_State = selectobjs[j];
        else if (selectobjs[j].id.indexOf("ddlCamp")>=0)
            Q8_Camp = selectobjs[j];
    }
   
    //validation for Question 7
    if (Q7_1.checked)
        bValid=true;
    
    else if (Q7_2.checked )
    {
        Q9_CampSession.value = trim(Q9_CampSession.value);
        Q10_StartDate.value = trim(Q10_StartDate.value);
        Q10_EndDate.value = trim(Q10_EndDate.value);
        //validation for the rest of the questions
        //for Question 10 
        if (Q8_Camp.selectedIndex==0)
        {
            strErrorMsg="<li>Please select a Camp</li>";
            bValid = false;
        }
        else if (Q9_CampSession.value=="") //for Question 11
        {
            strErrorMsg="<li>Please enter a Camp Session</li>";
            bValid = false;
        }
        else if (Q10_StartDate.value=="" || Q10_EndDate.value=="") //for Question 12
        {
            strErrorMsg="<li>Please enter dates in the mm/dd/yyyy format, or select the dates by clicking the calendar icons</li>";
            bValid = false;
        }
        else if (!ValidateDate(Q10_StartDate.value))
        {
            strErrorMsg="<li>Please enter a Valid Start Date in the mm/dd/yyyy format, or select the date by clicking the calendar icon</li>";
            bValid = false;
        }
        else if (!ValidateDate(Q10_EndDate.value))
        {
            strErrorMsg="<li>Please enter a Valid End Date in the mm/dd/yyyy format, or select the date by clicking the calendar icon</li>";
            bValid = false;
        }
       // else if (!CompareDates(currentDt,Q10_StartDate.value))
       // {
       //     strErrorMsg="<li>Start date can not be less than today's date</li>";
       //     bValid = false;
       // }
        else if (!CompareDates(Q10_StartDate.value,Q10_EndDate.value))
        {
            strErrorMsg="<li>Start Date should be less than the End Date</li>";
            bValid = false;
        }
        //Added by Ram (10/15/2009) related to allow "May, Jun, Jul, Aug, Sep" as session months
        
        else if (!CompareDates(startDate.value,Q10_StartDate.value))
        {            
            strErrorMsg="<li>"+campSeasonErrorMessage.value+"</li>";
            bValid = false;
        }
        else if (!CompareDates(Q10_StartDate.value,endDate.value))
        {   
            strErrorMsg="<li>"+campSeasonErrorMessage.value+"</li>";
            bValid = false;
        }
        else if (!CompareDates(startDate.value,Q10_EndDate.value))
        {   
            strErrorMsg="<li>"+campSeasonErrorMessage.value+"</li>";
            bValid = false;
        }
        else if (!CompareDates(Q10_EndDate.value,endDate.value))
        {   
            strErrorMsg="<li>"+campSeasonErrorMessage.value+"</li>";
            bValid = false;
        } 
        else
            bValid = true;
    }//end of else if
    else  //Question 7 is not answered
    {
        strErrorMsg="<li>Please answer Question No. 7</li>";
        bValid = false;
    }
    
    valObj.innerHTML = strErrorMsg;
    args.IsValid = bValid;
    return;
}

//for Question 9 for cincinaati and Question 7 for Middle sex, Q12 for Greenboro
//in Step 2 (Page 3) of URJ ///////////////////////
function windowURJopen()
{
    var arguments = windowURJopen.arguments;
    var RadioObj, k=0;
    var divctlsid = new Array();
    var bCheckedValue;
    
    //to get the input parameters passed to this method
    //1st parameter - Radiobutton obj
    //rest parameters - divobjects separated by comma
    for (var j=0; j<=arguments.length-1; j++)
    {
        switch (j)
        {
            case 0:
                RadioObj = arguments[j];
                break;
            default:
                divctlsid[k] = arguments[j];
                k=k+1;
                break;
        }
    }

    switch (RadioObj.value)
    {
        case "1":
            bCheckedValue=true;
            break;
        default:
            bCheckedValue = false;
            break;
    }
    
    
    //PanelStatus(windowopen.arguments);
     //to open up the info page
    if (bCheckedValue) 
        window.open('../../CampSearch_New.aspx','search','toolbar=no,status=no,scroll=no,width=800,height=400')
        var divobjs = new Array();
    divobjs = document.getElementsByTagName("div");
    
    //to disable the child nodes
    for (var i=0; i < divobjs.length-1; i++)
    {
        //to disable all the childnode of the panel
        
        for (var l=0; l<=divctlsid.length-1; l++)
        {   if(divobjs[i].id.indexOf(divctlsid[l])>=0)
            {
                if(!(divobjs[i].id.indexOf("PnlQ8")>=0))
                {
                    divobjs[i].disabled=bCheckedValue;
                    var child = new Array();
                    child = divobjs[i].childNodes;
                    for (var j=0; j<child.length-1; j++)
                    {   
                        if (child[j].id !=undefined) 
                        {
                            child[j].disabled = bCheckedValue;
                            //to set the value in the textbox and dropdown to the default value
                            if (child[j].type!=null && bCheckedValue==true)
                            {
                                if (child[j].type.indexOf("select")>=0) child[j].selectedIndex=0; //if it is dropdown
                                else child[j].value=""; //for text box
                            }
                            //this is used for Jwest (Step 2 - Page 3 where the start date and end date are displayed in labels)
                            if (child[j].id.indexOf("lblStartDate")>=0 || child[j].id.indexOf("lblEndDate")>=0)
                            {
                                if (bCheckedValue) child[j].innerText="";
                            }
                        }
                    }
                 }   
            }
        }
    }        
}


//*****************************END OF VALIDATION FOR URJ**********************************

//////********************** START OF VALIDATION FOR CAMPSAIRYLOUISE *******************

//to validate the Step2 (Page 3) of Camps AiryLouise Questionaire/////////////////////////
function VaildatePage3Step2_AiryLouise(sender,args)
{
    var Q7_1, Q7_2, Q7_3, Q7_4, Q8_State, Q8_Camp, Q9_CampSession, Q10_StartDate, Q10_EndDate;
    var inputobjs = document.getElementsByTagName("input");
    var selectobjs = document.getElementsByTagName("select");
    var valObj = document.getElementById(sender.id);
    var dt = new Date();
    var currentDt = dt.getMonth()+ 1 + "/" + dt.getDate() + "/" + dt.getYear();
    var bValid=false;
    var strErrorMsg="";
    
    var startDate = document.getElementById("ctl00_hdnCampSessionStartDate");
    var endDate = document.getElementById("ctl00_hdnCampSessionEndDate");
    var campSeasonErrorMessage = document.getElementById("ctl00_hdncampSeasonErrorMessage");
    
    for (var i = 0; i<inputobjs.length-1; i++)
    {
        if (inputobjs[i].id.indexOf("RadioButtonQ7Option1")>=0)
            Q7_1 = inputobjs[i];
        else if (inputobjs[i].id.indexOf("RadioButtonQ7Option2")>=0)
            Q7_2 = inputobjs[i];
//        else if (inputobjs[i].id.indexOf("RadioButtonQ7Option3")>=0)
//            Q7_3 = inputobjs[i];
//        else if (inputobjs[i].id.indexOf("RadioButtonQ7Option4")>=0)
//            Q7_4 = inputobjs[i];
        else if (inputobjs[i].id.indexOf("RadioButtonQ7Option2")>=0)
            Q7_2 = inputobjs[i];
        else if (inputobjs[i].id.indexOf("txtCampSession")>=0)
            Q9_CampSession = inputobjs[i];  
        else if (inputobjs[i].id.indexOf("txtStartDate")>=0)
            Q10_StartDate = inputobjs[i];
        else if (inputobjs[i].id.indexOf("txtEndDate")>=0)
            Q10_EndDate = inputobjs[i];
    } //end of for loop    
    
    Q8_Camp = selectobjs[0];
    //Q9_CampSession = selectobjs[1];
    //validation for Question 7
    if (Q7_1.checked)
        bValid=true;
    
    else if (Q7_2.checked )
    {
        //Q9_CampSession.value = trim(Q9_CampSession.value);
        Q10_StartDate.value = trim(Q10_StartDate.value);
        Q10_EndDate.value = trim(Q10_EndDate.value);
        //validation for the rest of the questions
        //for Question 10 
        if (Q8_Camp.selectedIndex==0)
        {
            strErrorMsg="<li>Please select a Camp</li>";
            bValid = false;
        }
        else if (Q9_CampSession.value=="") //for Question 11
        {
            strErrorMsg="<li>Please enter a Camp Session</li>";
            bValid = false;
        }
        else if (Q10_StartDate.value=="" || Q10_EndDate.value=="") //for Question 12
        {
            strErrorMsg="<li>Please enter dates in the mm/dd/yyyy format, or select the dates by clicking the calendar icons</li>";
            bValid = false;
        }
        else if (!ValidateDate(Q10_StartDate.value))
        {
            strErrorMsg="<li>Please enter a Valid Start Date in the mm/dd/yyyy format, or select the date by clicking the calendar icon</li>";
            bValid = false;
        }
        else if (!ValidateDate(Q10_EndDate.value))
        {
            strErrorMsg="<li>Please enter a Valid End Date in the mm/dd/yyyy format, or select the date by clicking the calendar icon</li>";
            bValid = false;
        }
        
        //Added by Ram (10/15/2009) related to allow "May, Jun, Jul, Aug, Sep" as session months
        
        else if (!CompareDates(startDate.value,Q10_StartDate.value))
        {            
            strErrorMsg="<li>"+campSeasonErrorMessage.value+"</li>";
            bValid = false;
        }
        else if (!CompareDates(Q10_StartDate.value,endDate.value))
        {   
            strErrorMsg="<li>"+campSeasonErrorMessage.value+"</li>";
            bValid = false;
        }
        else if (!CompareDates(startDate.value,Q10_EndDate.value))
        {   
            strErrorMsg="<li>"+campSeasonErrorMessage.value+"</li>";
            bValid = false;
        }
        else if (!CompareDates(Q10_EndDate.value,endDate.value))
        {   
            strErrorMsg="<li>"+campSeasonErrorMessage.value+"</li>";
            bValid = false;
        }
//        else if (!CompareDates(currentDt,Q10_StartDate.value))
//        {
//            strErrorMsg="<li>Start date can not be less than today's date</li>";
//            bValid = false;
//        }
        else if (!CompareDates(Q10_StartDate.value,Q10_EndDate.value))
        {
            strErrorMsg="<li>Start Date should be less than the End Date</li>";
            bValid = false;
        }
        else
            bValid = true;
    }//end of else if
    else  //Question 7 is not answered
    {
        strErrorMsg="<li>Please answer the Camp registration Question.</li>";
        bValid = false;
    }
    
    valObj.innerHTML = strErrorMsg;
    args.IsValid = bValid;
    return;
}
//************************* START OF VALIDATION FOR SPORTSACADAMY ********************
function VaildatePage3Step2_SportsAcadamy(sender,args)
{
    var Q7_1, Q7_2, Q7_3, Q7_4, Q8_State, Q8_Camp, Q9_CampSession, Q10_StartDate, Q10_EndDate;
    var inputobjs = document.getElementsByTagName("input");
    var selectobjs = document.getElementsByTagName("select");
    var valObj = document.getElementById(sender.id);
    var dt = new Date();
    var currentDt = dt.getMonth()+ 1 + "/" + dt.getDate() + "/" + dt.getYear();
    var bValid=false;
    var strErrorMsg="";
    
    var startDate = document.getElementById("ctl00_hdnCampSessionStartDate");
    var endDate = document.getElementById("ctl00_hdnCampSessionEndDate");
    var campSeasonErrorMessage = document.getElementById("ctl00_hdncampSeasonErrorMessage");
    
    for (var i = 0; i<inputobjs.length-1; i++)
    {
        if (inputobjs[i].id.indexOf("RadioButtonQ7Option1")>=0)
            Q7_1 = inputobjs[i];
        else if (inputobjs[i].id.indexOf("RadioButtonQ7Option2")>=0)
            Q7_2 = inputobjs[i];
//        else if (inputobjs[i].id.indexOf("RadioButtonQ7Option3")>=0)
//            Q7_3 = inputobjs[i];
//        else if (inputobjs[i].id.indexOf("RadioButtonQ7Option4")>=0)
//            Q7_4 = inputobjs[i];
        else if (inputobjs[i].id.indexOf("RadioButtonQ7Option2")>=0)
            Q7_2 = inputobjs[i];
        else if (inputobjs[i].id.indexOf("txtCampSession")>=0)
            Q9_CampSession = inputobjs[i];  
        else if (inputobjs[i].id.indexOf("txtStartDate")>=0)
            Q10_StartDate = inputobjs[i];
        else if (inputobjs[i].id.indexOf("txtEndDate")>=0)
            Q10_EndDate = inputobjs[i];
    } //end of for loop    
    
    Q8_Camp = selectobjs[0];
    //Q9_CampSession = selectobjs[1];
    //validation for Question 7
    if (Q7_1.checked)
        bValid=true;
    
    else if (Q7_2.checked )
    {
        Q9_CampSession.value = trim(Q9_CampSession.value);
        Q10_StartDate.value = trim(Q10_StartDate.value);
        Q10_EndDate.value = trim(Q10_EndDate.value);
        //validation for the rest of the questions
        //for Question 10 
        if (Q8_Camp.selectedIndex==0)
        {
            strErrorMsg="<li>Please select a Camp</li>";
            bValid = false;
        }
        else if (Q9_CampSession.value=="") //for Question 11
        {
            strErrorMsg="<li>Please enter a Camp Session</li>";
            bValid = false;
        }
        else if (Q10_StartDate.value=="" || Q10_EndDate.value=="") //for Question 12
        {
            strErrorMsg="<li>Please enter dates in the mm/dd/yyyy format, or select the dates by clicking the calendar icons</li>";
            bValid = false;
        }
        else if (!ValidateDate(Q10_StartDate.value))
        {
            strErrorMsg="<li>Please enter a Valid Start Date in the mm/dd/yyyy format, or select the date by clicking the calendar icon</li>";
            bValid = false;
        }
        else if (!ValidateDate(Q10_EndDate.value))
        {
            strErrorMsg="<li>Please enter a Valid End Date in the mm/dd/yyyy format, or select the date by clicking the calendar icon</li>";
            bValid = false;
        }
        
        //Added by Ram (10/15/2009) related to allow "May, Jun, Jul, Aug, Sep" as session months
        
        else if (!CompareDates(startDate.value,Q10_StartDate.value))
        {            
            strErrorMsg="<li>"+campSeasonErrorMessage.value+"</li>";
            bValid = false;
        }
        else if (!CompareDates(Q10_StartDate.value,endDate.value))
        {   
            strErrorMsg="<li>"+campSeasonErrorMessage.value+"</li>";
            bValid = false;
        }
        else if (!CompareDates(startDate.value,Q10_EndDate.value))
        {   
            strErrorMsg="<li>"+campSeasonErrorMessage.value+"</li>";
            bValid = false;
        }
        else if (!CompareDates(Q10_EndDate.value,endDate.value))
        {   
            strErrorMsg="<li>"+campSeasonErrorMessage.value+"</li>";
            bValid = false;
        }
//        else if (!CompareDates(currentDt,Q10_StartDate.value))
//        {
//            strErrorMsg="<li>Start date can not be less than today's date</li>";
//            bValid = false;
//        }
        else if (!CompareDates(Q10_StartDate.value,Q10_EndDate.value))
        {
            strErrorMsg="<li>Start Date should be less than the End Date</li>";
            bValid = false;
        }
        else
            bValid = true;
    }//end of else if
    else  //Question 7 is not answered
    {
        strErrorMsg="<li>Please answer the Camp registration Question.</li>";
        bValid = false;
    }
    
    valObj.innerHTML = strErrorMsg;
    args.IsValid = bValid;
    return;
}
function windowURJSportsAcadamyopen()
{
    var arguments = windowURJSportsAcadamyopen.arguments;
    var RadioObj, k=0;
    var divctlsid = new Array();
    var bCheckedValue;
    
    //to get the input parameters passed to this method
    //1st parameter - Radiobutton obj
    //rest parameters - divobjects separated by comma
    for (var j=0; j<=arguments.length-1; j++)
    {
        switch (j)
        {
            case 0:
                RadioObj = arguments[j];
                break;
            default:
                divctlsid[k] = arguments[j];
                k=k+1;
                break;
        }
    }

    switch (RadioObj.value)
    {
        case "1":
            bCheckedValue=true;
            break;
        default:
            bCheckedValue = false;
            break;
    }
    
    
    //PanelStatus(windowopen.arguments);
     //to open up the info page
    if (bCheckedValue) 
        window.open('../../../CampSearch_New.aspx','search','toolbar=no,status=no,scroll=no,width=800,height=400')
        var divobjs = new Array();
    divobjs = document.getElementsByTagName("div");
    
    //to disable the child nodes
    for (var i=0; i < divobjs.length-1; i++)
    {
        //to disable all the childnode of the panel
        
        for (var l=0; l<=divctlsid.length-1; l++)
        {   if(divobjs[i].id.indexOf(divctlsid[l])>=0)
            {
                if(!(divobjs[i].id.indexOf("PnlQ8")>=0))
                {
                    divobjs[i].disabled=bCheckedValue;
                    var child = new Array();
                    child = divobjs[i].childNodes;
                    for (var j=0; j<child.length-1; j++)
                    {   
                        if (child[j].id !=undefined) 
                        {
                            child[j].disabled = bCheckedValue;
                            //to set the value in the textbox and dropdown to the default value
                            if (child[j].type!=null && bCheckedValue==true)
                            {
                                if (child[j].type.indexOf("select")>=0) child[j].selectedIndex=0; //if it is dropdown
                                else child[j].value=""; //for text box
                            }
                            //this is used for Jwest (Step 2 - Page 3 where the start date and end date are displayed in labels)
                            if (child[j].id.indexOf("lblStartDate")>=0 || child[j].id.indexOf("lblEndDate")>=0)
                            {
                                if (bCheckedValue) child[j].innerText="";
                            }
                        }
                    }
                 }   
            }
        }
    }        
}
//to open up a calendar
function ShowAcadamyCalendar(txtBoxId)
{
    window.open('../../../Calendar.aspx?txtBox=' + txtBoxId,'Calendar','toolbar=no,status=no,titlebar=no,scroll=no,width=200,height=190,left=610,top=420');
    //window.showModalDialog('../../Calendar.aspx?txtBox=' + txtBoxId,'Calendar','toolbar=no,status=no,titlebar=no,scroll=no,width=200,height=190,left=610,top=420');
    return false;
}
//************************* END OF VALIDATION FOR SPORTSACADAMY ********************
//************************* END OF VALIDATION FOR CAMPSAIRYLOUISE ********************

//to validate the Step2 (Page 3) of Bnai Brith Questionaire/////////////////////////
function VaildatePage3Step2_Bnai(sender,args)
{
    var Q7_1, Q7_2, Q7_3, Q7_4, Q8_State, Q8_Camp, Q9_CampSession, Q10_StartDate, Q10_EndDate;
    var inputobjs = document.getElementsByTagName("input");
    var selectobjs = document.getElementsByTagName("select");
    var valObj = document.getElementById(sender.id);
    var dt = new Date();
    var currentDt = dt.getMonth()+ 1 + "/" + dt.getDate() + "/" + dt.getYear();
    var bValid=false;
    var strErrorMsg="";
    
    var startDate = document.getElementById("ctl00_hdnCampSessionStartDate");
    var endDate = document.getElementById("ctl00_hdnCampSessionEndDate");
    var campSeasonErrorMessage = document.getElementById("ctl00_hdncampSeasonErrorMessage");
    
    for (var i = 0; i<inputobjs.length-1; i++)
    {
        if (inputobjs[i].id.indexOf("RadioButtonQ7Option1")>=0)
            Q7_1 = inputobjs[i];
        else if (inputobjs[i].id.indexOf("RadioButtonQ7Option2")>=0)
            Q7_2 = inputobjs[i];
//        else if (inputobjs[i].id.indexOf("RadioButtonQ7Option3")>=0)
//            Q7_3 = inputobjs[i];
//        else if (inputobjs[i].id.indexOf("RadioButtonQ7Option4")>=0)
//            Q7_4 = inputobjs[i];
        else if (inputobjs[i].id.indexOf("RadioButtonQ7Option2")>=0)
            Q7_2 = inputobjs[i];
        //else if (inputobjs[i].id.indexOf("txtCampSession")>=0)
            //Q9_CampSession = inputobjs[i];  
        else if (inputobjs[i].id.indexOf("txtStartDate")>=0)
            Q10_StartDate = inputobjs[i];
        else if (inputobjs[i].id.indexOf("txtEndDate")>=0)
            Q10_EndDate = inputobjs[i];
    } //end of for loop    
    
    Q8_Camp = selectobjs[0];
    Q9_CampSession = selectobjs[1];
    //validation for Question 7
    if (Q7_1.checked)
        bValid=true;
    
    else if (Q7_2.checked )
    {
        //Q9_CampSession.value = trim(Q9_CampSession.value);
        Q10_StartDate.value = trim(Q10_StartDate.value);
        Q10_EndDate.value = trim(Q10_EndDate.value);
        //validation for the rest of the questions
        //for Question 10 
        if (Q8_Camp.selectedIndex==0)
        {
            strErrorMsg="<li>Please select a Camp</li>";
            bValid = false;
        }
        else if (Q9_CampSession.selectedIndex==0) //for Question 11
        {
            strErrorMsg="<li>Please select a Camp Session</li>";
            bValid = false;
        }
        else if (Q10_StartDate.value=="" || Q10_EndDate.value=="") //for Question 12
        {
            strErrorMsg="<li>Please enter dates in the mm/dd/yyyy format, or select the dates by clicking the calendar icons</li>";
            bValid = false;
        }
        else if (!ValidateDate(Q10_StartDate.value))
        {
            strErrorMsg="<li>Please enter a Valid Start Date in the mm/dd/yyyy format, or select the date by clicking the calendar icon</li>";
            bValid = false;
        }
        else if (!ValidateDate(Q10_EndDate.value))
        {
            strErrorMsg="<li>Please enter a Valid End Date in the mm/dd/yyyy format, or select the date by clicking the calendar icon</li>";
            bValid = false;
        }
        
        //Added by Ram (10/15/2009) related to allow "May, Jun, Jul, Aug, Sep" as session months
        
        else if (!CompareDates(startDate.value,Q10_StartDate.value))
        {            
            strErrorMsg="<li>"+campSeasonErrorMessage.value+"</li>";
            bValid = false;
        }
        else if (!CompareDates(Q10_StartDate.value,endDate.value))
        {   
            strErrorMsg="<li>"+campSeasonErrorMessage.value+"</li>";
            bValid = false;
        }
        else if (!CompareDates(startDate.value,Q10_EndDate.value))
        {   
            strErrorMsg="<li>"+campSeasonErrorMessage.value+"</li>";
            bValid = false;
        }
        else if (!CompareDates(Q10_EndDate.value,endDate.value))
        {   
            strErrorMsg="<li>"+campSeasonErrorMessage.value+"</li>";
            bValid = false;
        }
//        else if (!CompareDates(currentDt,Q10_StartDate.value))
//        {
//            strErrorMsg="<li>Start date can not be less than today's date</li>";
//            bValid = false;
//        }
        else if (!CompareDates(Q10_StartDate.value,Q10_EndDate.value))
        {
            strErrorMsg="<li>Start Date should be less than the End Date</li>";
            bValid = false;
        }
        else
            bValid = true;
    }//end of else if
    else  //Question 7 is not answered
    {
        strErrorMsg="<li>Please answer Question No. 5</li>";
        bValid = false;
    }
    
    valObj.innerHTML = strErrorMsg;
    args.IsValid = bValid;
    return;
}
//for Question 9 for cincinaati and Question 7 for Middle sex, Q12 for Greenboro
//in Step 2 (Page 3) of URJ ///////////////////////
function windowsavexit()
{
    window.open('CampMessage.aspx','search','toolbar=no,status=no,scroll=no,width=800,height=400');
    return true;
}
function windowBnaiopen()
{
    var arguments = windowBnaiopen.arguments;
    var RadioObj, k=0;
    var divctlsid = new Array();
    var bCheckedValue;
    
    //to get the input parameters passed to this method
    //1st parameter - Radiobutton obj
    //rest parameters - divobjects separated by comma
    for (var j=0; j<=arguments.length-1; j++)
    {
        switch (j)
        {
            case 0:
                RadioObj = arguments[j];
                break;
            default:
                divctlsid[k] = arguments[j];
                k=k+1;
                break;
        }
    }

    switch (RadioObj.value)
    {
        case "1":
            bCheckedValue=true;
            break;
        default:
            bCheckedValue = false;
            break;
    }
 
    //PanelStatus(windowopen.arguments);
     //to open up the info page
     
    if (bCheckedValue) 
        window.open('CampMessage.aspx','search','toolbar=no,status=no,scroll=no,width=800,height=400')
        var divobjs = new Array();
    divobjs = document.getElementsByTagName("div");
    
    //to disable the child nodes
    for (var i=0; i <= divobjs.length-1; i++)
    {
        //to disable all the childnode of the panel
        
        for (var l=0; l<=divctlsid.length-1; l++)
        {   if(divobjs[i].id.indexOf(divctlsid[l])>=0)
            {
                if(!(divobjs[i].id.indexOf("PnlQ8")>=0))
                {
                    divobjs[i].disabled=bCheckedValue;
                    var child = new Array();
                    child = divobjs[i].childNodes;
                    for (var j=0; j<child.length-1; j++)
                    {   
                        if (child[j].id !=undefined) 
                        {
                            child[j].disabled = bCheckedValue;
                            //to set the value in the textbox and dropdown to the default value
                            if (child[j].type!=null && bCheckedValue==true)
                            {
                                if (child[j].type.indexOf("select")>=0) child[j].selectedIndex=0; //if it is dropdown
                                else child[j].value=""; //for text box
                            }
                            //this is used for Jwest (Step 2 - Page 3 where the start date and end date are displayed in labels)
                            if (child[j].id.indexOf("lblStartDate")>=0 || child[j].id.indexOf("lblEndDate")>=0)
                            {
                                if (bCheckedValue) child[j].innerText="";
                            }
                        }
                    }
                 }   
            }
        }
    }        
}

//*****************************END OF VALIDATION FOR BNai Brith**********************************

//*****************************BEGIN OF VALIDATION FOR Philedalphia**********************************
//to validate the Step2 (Page 2) for NY /////////////////////////
function ValidatePage2Step2_Philadelphia(sender,args)
{
    var inputobjs = document.getElementsByTagName("input");
    var selectobjs = document.getElementsByTagName("select");
    var Q3_1, Q3_2, Q4_1, Q4_2, Q5_1, Q5_2, Q6, Q7_1, Q7_2, Q8_1, Q8_2, Q9, Q10;
    Q9 = new Array();
    var j=0;
    var valobj = document.getElementById(sender.id);
    var hdnYearCount;
    for (var i = 0; i< inputobjs.length-1; i++)
    {
        //for Q3_1
        if (inputobjs[i].id.indexOf("RadioBtnQ3_0")>=0)
        {
            Q3_1 = inputobjs[i];
        }
        //for Q3_2
        if (inputobjs[i].id.indexOf("RadioBtnQ3_1")>=0)
        {
            Q3_2 = inputobjs[i];
        }
        
        //for Q4_1
        if (inputobjs[i].id.indexOf("RadioBtnQ4_0")>=0)
        {
            Q4_1 = inputobjs[i];
        }
        
        //for Q4_2
        if (inputobjs[i].id.indexOf("RadioBtnQ4_1")>=0)
        {
            Q4_2 = inputobjs[i];
        }
        
        //for Q5_1
        if (inputobjs[i].id.indexOf("RadioBtnQ5_0")>=0)
        {
            Q5_1 = inputobjs[i];
        }
        
        //for Q5_2
        if (inputobjs[i].id.indexOf("RadioBtnQ5_1")>=0)
        {
            Q5_2 = inputobjs[i];
        }
        
         //for Q7_1
//        if (inputobjs[i].id.indexOf("RadioBtnQ7_0")>=0)
//        {
//            Q7_1 = inputobjs[i];
//        }
//        
//        //for Q7_2
//        if (inputobjs[i].id.indexOf("RadioBtnQ7_1")>=0)
//        {
//            Q7_2 = inputobjs[i];
//        }  
        
        // for txtSynagogueReferral 
        
//        if (inputobjs[i].id.indexOf("txtOtherSynagogue")>=0)
//        {
//            Q8_2 = inputobjs[i];
//        }      

        //for Q9
        if (inputobjs[i].id.indexOf("RadioBtnQ9")>=0)
        {
            Q9[j] = inputobjs[i];
            j=j+1;
        }      

        //for Q10
        if (inputobjs[i].id.indexOf("txtCamperSchool")>=0)
        {
            Q10 = inputobjs[i];
        }
        
    }  //end of for loop
    
    //to get the <select> objects for Q8 and Q10
    for (var i = 0; i<= selectobjs.length-1; i++)
    {
        //for Q6
         if (selectobjs[i].id.indexOf("ddlGrade")>=0)
        {
            Q6 = selectobjs[i];
        } 
        
        //for Q8
//        if (selectobjs[i].id.indexOf("ddlSynagogue")>=0)
//        {
//            Q8_1 = selectobjs[i];
//        }         

    }

    //validate Q3
    if (Q3_1.checked==false && Q3_2.checked==false)
    {
        valobj.innerHTML = "<ul><li>Please answer Question No. 1</li></ul>";
        args.IsValid=false;
        return;
    }
    else if (Q3_1.checked) // if yes is checked
    {
    var returnVal = ValidateSynagogueAndJCCQuestion(inputobjs,selectobjs,valobj,false);
            if(returnVal == false)
            {
                args.IsValid = false;
                return;
            }
         //for Question 7
//        if (Q7_1.checked==false && Q7_2.checked==false)
//        {
//             valobj.innerHTML = "<li>Please answer Question No. 5</li>";
//             args.IsValid=false;
//             return;
//        }
//        if(Q7_1.checked==true)
//        {
//           //validate synagogue
//            if (Q8_1.selectedIndex==0)
//            {
//                valobj.innerHTML = "<li>Please select the Synagogue</li>";
//                args.IsValid=false;
//                return;
//            }  
//            else if(Q8_1.options[Q8_1.selectedIndex].text == "OTHER")
//            {
//                //referral code
//                if (trim(Q8_2.value)=="")
//                {
//                    valobj.innerHTML = "<li>Please enter the Synagogue name.</li>";
//                    args.IsValid=false;
//                    return;
//                }
//            }
//        }
    }
    else if (Q3_2.checked)  //if "no" is checked
    {
        //to validate for Q4 and Q5
        if (Q4_1.checked==false && Q4_2.checked==false)
        {
             valobj.innerHTML = "<li>Please answer Question No. 2</li>";
             args.IsValid=false;
             return;
        }
        else if (Q4_1.checked)
        {
            if (Q5_1.checked==false && Q5_2.checked==false)
            {
                 valobj.innerHTML = "<li>Please answer Question No. 3</li>";
                 args.IsValid=false;
                 return;
            }
            else if (Q5_1.checked)
            {
                //for Question 7
//                if (Q7_1.checked==false && Q7_2.checked==false)
//                {
//                     valobj.innerHTML = "<li>Please answer Question No. 5</li>";
//                     args.IsValid=false;
//                     return;
//                }
                var returnVal = ValidateSynagogueAndJCCQuestion(inputobjs,selectobjs,valobj,false);
                if(returnVal == false)
                {
                    args.IsValid = false;
                    return;
                }
                //validate synagogue
                
//                if (Q8_1.selectedIndex==0 && Q7_1.checked)
//                {
//                    valobj.innerHTML = "<li>Please select the Synagogue</li>";
//                    args.IsValid=false;
//                    return;
//                }  
//                //referral code
//                if (trim(Q8_2.value)=="" && Q8_1.options[Q8_1.selectedIndex].text == "OTHER")
//                {
//                    valobj.innerHTML = "<li>Please enter the Synagogue name.</li>";
//                    args.IsValid=false;
//                    return;
//                }
            }
            else if(Q5_2.checked)
            {
                var returnVal = ValidateSynagogueAndJCCQuestion(inputobjs,selectobjs,valobj,false);
                if(returnVal == false)
                {
                    args.IsValid = false;
                    return;
                }
            }
        }
        else if(Q4_2.checked)
        {
            var returnVal = ValidateSynagogueAndJCCQuestion(inputobjs,selectobjs,valobj,false);
            if(returnVal == false)
            {
                args.IsValid = false;
                return;
            }
        }
        

    }
    
    //validate Q6
    if (Q6.selectedIndex==0)
    {
        valobj.innerHTML = "<li>Please select the Grade</li>";
        args.IsValid=false;
        return;
    }   
    
    //for Question 9
    var bQ9Checked = false;
    for (var j =0 ; j<= Q9.length-1; j++)
    {
        if (Q9[j].checked) //Q9 has been answered
        {
            bQ9Checked=true;
            //for Question 11
            if (trim(Q10.value)=="" && Q9[j].value!=3)
            {
                valobj.innerHTML = "<li>Please enter Name of the School</li>";
                args.IsValid=false;
                return;
            }   
        }
    }
    
    // Referral code must be 4 digits
   /*  if (!(trim(Q8_2.value)==""))
    {
        if (!IsNumeric(trim(Q8_2.value)))
        {
            valobj.innerHTML = "<ul><li>Please enter valid referal code </li></ul>";
            args.IsValid=false;
            return;
 
        }   
    }
    
    // Referral code must be 4 digits
   if (!(trim(Q8_2.value)==""))
    {
        var refcode;
        refcode=trim(Q8_2.value);
        if (refcode.length<4)
        {
            valobj.innerHTML = "<ul><li>Please enter valid 4 digit referal code </li></ul>";
            args.IsValid=false;
            return;
 
        }   
    }
*/

    
    //if Q9 is not answered
    if (!bQ9Checked)
    {
         valobj.innerHTML = "<li>Please answer Question No. 6</li>";
         args.IsValid=false;
         return;
    }  
  
    args.IsValid = true;
    return;
}
//*****************************END OF VALIDATION FOR Philedalphia**********************************

//*****************************BEGIN OF VALIDATION FOR Cleveland**********************************
//to validate the Step2 (Page 2) for NY /////////////////////////
function ValidatePage2Step2_Cleveland(sender,args)
{
    var inputobjs = document.getElementsByTagName("input");
    var selectobjs = document.getElementsByTagName("select");
    var Q3_1, Q3_2, Q4_1, Q4_2, Q5_1, Q5_2, Q6, Q7_1, Q7_2, Q8_1, Q8_2, Q9, Q10;
    Q9 = new Array();
    var j=0;
    var valobj = document.getElementById(sender.id);
    var hdnYearCount;
    for (var i = 0; i< inputobjs.length-1; i++)
    {
        //for Q3_1
        if (inputobjs[i].id.indexOf("RadioBtnQ3_0")>=0)
        {
            Q3_1 = inputobjs[i];
        }
        //for Q3_2
        if (inputobjs[i].id.indexOf("RadioBtnQ3_1")>=0)
        {
            Q3_2 = inputobjs[i];
        }
        
        //for Q4_1
//        if (inputobjs[i].id.indexOf("RadioBtnQ4_0")>=0)
//        {
//            Q4_1 = inputobjs[i];
//        }
//        
//        //for Q4_2
//        if (inputobjs[i].id.indexOf("RadioBtnQ4_1")>=0)
//        {
//            Q4_2 = inputobjs[i];
//        }
        
        //for Q5_1
//        if (inputobjs[i].id.indexOf("RadioBtnQ5_0")>=0)
//        {
//            Q5_1 = inputobjs[i];
//        }
//        
//        //for Q5_2
//        if (inputobjs[i].id.indexOf("RadioBtnQ5_1")>=0)
//        {
//            Q5_2 = inputobjs[i];
//        }
        
         //for Q7_1
//        if (inputobjs[i].id.indexOf("RadioBtnQ7_0")>=0)
//        {
//            Q7_1 = inputobjs[i];
//        }
//        
//        //for Q7_2
//        if (inputobjs[i].id.indexOf("RadioBtnQ7_1")>=0)
//        {
//            Q7_2 = inputobjs[i];
//        }  
//        
        // for txtSynagogueReferral 
        
//        if (inputobjs[i].id.indexOf("txtOtherSynagogue")>=0)
//        {
//            Q8_2 = inputobjs[i];
//        }      

        //for Q9
        if (inputobjs[i].id.indexOf("RadioBtnQ9")>=0)
        {
            Q9[j] = inputobjs[i];
            j=j+1;
        }      

        //for Q10
        if (inputobjs[i].id.indexOf("txtCamperSchool")>=0)
        {
            Q10 = inputobjs[i];
        }
        
    }  //end of for loop
    
    //to get the <select> objects for Q8 and Q10
    for (var i = 0; i<= selectobjs.length-1; i++)
    {
        //for Q6
         if (selectobjs[i].id.indexOf("ddlGrade")>=0)
        {
            Q6 = selectobjs[i];
        } 
        
        //for Q8
//        if (selectobjs[i].id.indexOf("ddlSynagogue")>=0)
//        {
//            Q8_1 = selectobjs[i];
//        }         

    }

    //validate Q3
    if (Q3_1.checked==false && Q3_2.checked==false)
    {
        valobj.innerHTML = "<ul><li>Please answer Question No. 1</li></ul>";
        args.IsValid=false;
        return;
    }
//    else if (Q3_1.checked) // if yes is checked
    if(Q3_1.checked==true || Q3_2.checked==true)
    {
         //For Synagogue and JCC Question
        var returnVal = ValidateSynagogueAndJCCQuestion(inputobjs,selectobjs,valobj,false);
        if(returnVal == false)
        {
            args.IsValid = false;
            return;
        }
    
//         //for Question 7
//        if (Q7_1.checked==false && Q7_2.checked==false)
//        {
//             valobj.innerHTML = "<li>Please answer Question No. 5</li>";
//             args.IsValid=false;
//             return;
//        }
//        if(Q7_1.checked==true)
//        {
//           //validate synagogue
//            if (Q8_1.selectedIndex==0)
//            {
//                valobj.innerHTML = "<li>Please select the Synagogue</li>";
//                args.IsValid=false;
//                return;
//            }  
//            else if(Q8_1.options[Q8_1.selectedIndex].text == "OTHER")
//            {
//                //referral code
//                if (trim(Q8_2.value)=="")
//                {
//                    valobj.innerHTML = "<li>Please enter the Synagogue name.</li>";
//                    args.IsValid=false;
//                    return;
//                }
//            }
//        }
    }
//    else if (Q3_2.checked)  //if "no" is checked
//    {
//        //to validate for Q4 and Q5
//        if (Q4_1.checked==false && Q4_2.checked==false)
//        {
//             valobj.innerHTML = "<li>Please answer Question No. 3</li>";
//             args.IsValid=false;
//             return;
//        }
//        else if (Q4_1.checked)
//        {
//            if (Q5_1.checked==false && Q5_2.checked==false)
//            {
//                 valobj.innerHTML = "<li>Please answer Question No. 4</li>";
//                 args.IsValid=false;
//                 return;
//            }
//            else if (Q5_1.checked)
//            {
//             //For Synagogue and JCC Question
//                var returnVal = ValidateSynagogueAndJCCQuestion(inputobjs,selectobjs,valobj,false);
//                if(returnVal == false)
//                {
//                    args.IsValid = false;
//                    return;
//                }
////                //for Question 7
////                if (Q7_1.checked==false && Q7_2.checked==false)
////                {
////                     valobj.innerHTML = "<li>Please answer Question No. 5</li>";
////                     args.IsValid=false;
////                     return;
////                }
////                //validate synagogue
////                if (Q8_1.selectedIndex==0 && Q7_1.checked)
////                {
////                    valobj.innerHTML = "<li>Please select the Synagogue</li>";
////                    args.IsValid=false;
////                    return;
////                }  
////                //referral code
////                if (trim(Q8_2.value)=="" && Q8_1.options[Q8_1.selectedIndex].text == "OTHER")
////                {
////                    valobj.innerHTML = "<li>Please enter the Synagogue name.</li>";
////                    args.IsValid=false;
////                    return;
////                }
//            }
//        }

//    }
//    
    //validate Q6
    if (Q6.selectedIndex==0)
    {
        valobj.innerHTML = "<li>Please select the Grade</li>";
        args.IsValid=false;
        return;
    }   
    
    //for Question 9
    var bQ9Checked = false;
    for (var j =0 ; j<= Q9.length-1; j++)
    {
        if (Q9[j].checked) //Q9 has been answered
        {
            bQ9Checked=true;
            //for Question 11
            if (trim(Q10.value)=="" && Q9[j].value!=3)
            {
                valobj.innerHTML = "<li>Please enter Name of the School</li>";
                args.IsValid=false;
                return;
            }   
        }
    }
    
    // Referral code must be 4 digits
   /*  if (!(trim(Q8_2.value)==""))
    {
        if (!IsNumeric(trim(Q8_2.value)))
        {
            valobj.innerHTML = "<ul><li>Please enter valid referal code </li></ul>";
            args.IsValid=false;
            return;
 
        }   
    }
    
    // Referral code must be 4 digits
   if (!(trim(Q8_2.value)==""))
    {
        var refcode;
        refcode=trim(Q8_2.value);
        if (refcode.length<4)
        {
            valobj.innerHTML = "<ul><li>Please enter valid 4 digit referal code </li></ul>";
            args.IsValid=false;
            return;
 
        }   
    }
*/

    
    //if Q9 is not answered
    if (!bQ9Checked)
    {
         valobj.innerHTML = "<li>Please answer Question No. 4</li>";
         args.IsValid=false;
         return;
    }  
  
    args.IsValid = true;
    return;
}
//*****************************END OF VALIDATION FOR Cleveland**********************************

//********************************* BEGIN OF VALIDATION FOR Washington DC **************************
function ValidatePage2Step2_Washington(sender,args)
{
    var inputobjs = document.getElementsByTagName("input");
    var selectobjs = document.getElementsByTagName("select");
    var Q3_1, Q3_2, Q4_1, Q4_2, Q5_1, Q5_2, Q6, Q7_1, Q7_2, Q8_1, Q8_2, Q9, Q10,Q11_1,Q11_2,Q12;
    Q9 = new Array();
    var j=0;
    var valobj = document.getElementById(sender.id);
    var hdnYearCount;
    for (var i = 0; i< inputobjs.length-1; i++)
    {
        //for Q3_1
        if (inputobjs[i].id.indexOf("RadioBtnQ3_0")>=0)
        {
            Q3_1 = inputobjs[i];
        }
        //for Q3_2
        if (inputobjs[i].id.indexOf("RadioBtnQ3_1")>=0)
        {
            Q3_2 = inputobjs[i];
        }
        
        //for Q7_1
//        if (inputobjs[i].id.indexOf("RadioBtnQ7_0")>=0)
//        {
//            Q7_1 = inputobjs[i];
//        }
//        
//        //for Q7_2
//        if (inputobjs[i].id.indexOf("RadioBtnQ7_1")>=0)
//        {
//            Q7_2 = inputobjs[i];
//        }  
        
        // for txtSynagogueReferral 
        
//        if (inputobjs[i].id.indexOf("txtSynagogueReferral")>=0)
//        {
//            Q8_2 = inputobjs[i];
//        }      

        //for Q9
        if (inputobjs[i].id.indexOf("RadioBtnQ9")>=0)
        {
            Q9[j] = inputobjs[i];
            j=j+1;
        }      

        //for Q10
        if (inputobjs[i].id.indexOf("txtCamperSchool")>=0)
        {
            Q10 = inputobjs[i];
        }
        //for Q11_1
        if (inputobjs[i].id.indexOf("RadioBtnQ12_0")>=0)
        {
            Q11_1 = inputobjs[i];
        }
        
        //for Q11_2
        if (inputobjs[i].id.indexOf("RadioBtnQ12_1")>=0)
        {
            Q11_2 = inputobjs[i];
        } 
        //for Q12
        if (inputobjs[i].id.indexOf("txtDayCamp")>=0)
        {
            Q12 = inputobjs[i];
        }
    }  //end of for loop
    
    //to get the <select> objects for Q8 and Q10
    for (var i = 0; i<= selectobjs.length-1; i++)
    {
        //for Q6
         if (selectobjs[i].id.indexOf("ddlGrade")>=0)
        {
            Q6 = selectobjs[i];
        } 
        
        //for Q8
//        if (selectobjs[i].id.indexOf("ddlSynagogue")>=0)
//        {
//            Q8_1 = selectobjs[i];
//        }         

    }

    //validate Q3
    if (Q3_1.checked==false && Q3_2.checked==false)
    {
        valobj.innerHTML = "<ul><li>Please answer Question No. 1</li></ul>";
        args.IsValid=false;
        return;
    }
//    else if (Q3_1.checked) // if yes is checked
//    {
//         //for Question 7
//        if (Q7_1.checked==false && Q7_2.checked==false)
//        {
//             valobj.innerHTML = "<li>Please answer Question No. 3</li>";
//             args.IsValid=false;
//             return;
//        }
//           //validate synagogue
//        if (Q7_1.checked==true && Q8_1.selectedIndex==0)
//        {
//            valobj.innerHTML = "<li>Please select the Synagogue</li>";
//            args.IsValid=false;
//            return;
//        }  
//        //referral code
////        if (trim(Q8_2.value)=="")
////        {
////            valobj.innerHTML = "<li>Please enter the referral code </li>";
////            args.IsValid=false;
////            return;
////        }  

//        
//    }
       var returnVal = ValidateSynagogueAndJCCQuestion(inputobjs,selectobjs,valobj,false);
    if(returnVal == false)
    {
        args.IsValid = false;
        return;
    }
    //validate Q6
    if (Q6.selectedIndex==0)
    {
        valobj.innerHTML = "<li>Please select the Grade</li>";
        args.IsValid=false;
        return;
    }   
    
    //for Question 9
    var bQ9Checked = false;
    for (var j =0 ; j<= Q9.length-1; j++)
    {
        if (Q9[j].checked) //Q9 has been answered
        {
            bQ9Checked=true;
            //for Question 11
            if (trim(Q10.value)=="" && Q9[j].value!=3)
            {
                valobj.innerHTML = "<li>Please enter Name of the School</li>";
                args.IsValid=false;
                return;
            }   
        }
    }
    
//    // Referral code must be 4 digits
//    if (!(trim(Q8_2.value)==""))
//    {
//        if (!IsNumeric(trim(Q8_2.value)))
//        {
//            valobj.innerHTML = "<ul><li>Please enter valid referal code </li></ul>";
//            args.IsValid=false;
//            return;
// 
//        }   
//    }
//    
//    // Referral code must be 4 digits
//    if (!(trim(Q8_2.value)==""))
//    {
//        var refcode;
//        refcode=trim(Q8_2.value);
//        if (refcode.length<4)
//        {
//            valobj.innerHTML = "<ul><li>Please enter valid 4 digit referal code </li></ul>";
//            args.IsValid=false;
//            return;
// 
//        }   
//    }


    
    //if Q9 is not answered
    if (!bQ9Checked)
    {
         valobj.innerHTML = "<li>Please answer Question No. 4</li>";
         args.IsValid=false;
         return;
    }  
  if (Q11_1.checked==false && Q11_2.checked==false)
    {
        valobj.innerHTML = "<ul><li>Please answer Question No. 6</li></ul>";
        args.IsValid=false;
        return;
    }
    else if (Q11_1.checked) // if yes is checked
    {
        if(trim(Q12.value)=="")
        {
            valobj.innerHTML = "<ul><li>Please enter Day Camp attended </li></ul>";
            args.IsValid=false;
            return;
        }
    }
    args.IsValid = true;
    return;
}

function VaildatePage3Step2_Washington(sender,args)
{
    var Q7_1, Q7_2, Q7_3, Q7_4, Q8_State, Q8_Camp, Q9_CampSession, Q10_StartDate, Q10_EndDate;
    var inputobjs = document.getElementsByTagName("input");
    var selectobjs = document.getElementsByTagName("select");
    var valObj = document.getElementById(sender.id);
    var dt = new Date();
    var currentDt = dt.getMonth()+ 1 + "/" + dt.getDate() + "/" + dt.getYear();
    var bValid=false;
    var strErrorMsg="";
    var startDate = document.getElementById("ctl00_hdnCampSessionStartDate");
    var endDate = document.getElementById("ctl00_hdnCampSessionEndDate");
    var campSeasonErrorMessage = document.getElementById("ctl00_hdncampSeasonErrorMessage");
    
    for (var i = 0; i<inputobjs.length-1; i++)
    {
        if (inputobjs[i].id.indexOf("RadioButtonQ7Option1")>=0)
            Q7_1 = inputobjs[i];
        else if (inputobjs[i].id.indexOf("RadioButtonQ7Option2")>=0)
            Q7_2 = inputobjs[i];
//        else if (inputobjs[i].id.indexOf("RadioButtonQ7Option3")>=0)
//            Q7_3 = inputobjs[i];
//        else if (inputobjs[i].id.indexOf("RadioButtonQ7Option4")>=0)
//            Q7_4 = inputobjs[i];
        else if (inputobjs[i].id.indexOf("RadioButtonQ7Option2")>=0)
            Q7_2 = inputobjs[i];
        else if (inputobjs[i].id.indexOf("txtCampSession")>=0)
            Q9_CampSession = inputobjs[i];  
        else if (inputobjs[i].id.indexOf("txtStartDate")>=0)
            Q10_StartDate = inputobjs[i];
        else if (inputobjs[i].id.indexOf("txtEndDate")>=0)
            Q10_EndDate = inputobjs[i];
    } //end of for loop
    
    //to select the dropdown objs
    for (var j=0; j<=selectobjs.length-1; j++)
    {
        /*if (selectobjs[j].id.indexOf("ddlState")>=0)
            Q8_State = selectobjs[j];
        else*/
         if (selectobjs[j].id.indexOf("ddlCamp")>=0)
            Q8_Camp = selectobjs[j];
    }
   
    //validation for Question 7
    if (Q7_1.checked)
        bValid=true;
    
    else if (Q7_2.checked )
    {
        Q9_CampSession.value = trim(Q9_CampSession.value);
        Q10_StartDate.value = trim(Q10_StartDate.value);
        Q10_EndDate.value = trim(Q10_EndDate.value);
        //validation for the rest of the questions
        //for Question 10 
        if (Q8_Camp.selectedIndex==0)
        {
            strErrorMsg="<li>Please select a Camp</li>";
            bValid = false;
        }
        else if (Q9_CampSession.value=="") //for Question 11
        {
            strErrorMsg="<li>Please enter a Camp Session</li>";
            bValid = false;
        }
        else if (Q10_StartDate.value=="" || Q10_EndDate.value=="") //for Question 12
        {
            strErrorMsg="<li>Please enter dates in the mm/dd/yyyy format, or select the dates by clicking the calendar icons</li>";
            bValid = false;
        }
        else if (!ValidateDate(Q10_StartDate.value))
        {
            strErrorMsg="<li>Please enter a Valid Start Date in the mm/dd/yyyy format, or select the date by clicking the calendar icon</li>";
            bValid = false;
        }
        else if (!ValidateDate(Q10_EndDate.value))
        {
            strErrorMsg="<li>Please enter a Valid End Date in the mm/dd/yyyy format, or select the date by clicking the calendar icon</li>";
            bValid = false;
        }
        //else if (!CompareDates(currentDt,Q10_StartDate.value))
        //{
        //    strErrorMsg="<li>Start date can not be less than today's date</li>";
        //    bValid = false;
        //}
        else if (!CompareDates(Q10_StartDate.value,Q10_EndDate.value))
        {
            strErrorMsg="<li>Start Date should be less than the End Date</li>";
            bValid = false;
        }
        //Added by Ram (10/15/2009) related to allow "May, Jun, Jul, Aug, Sep" as session months
        
        else if (!CompareDates(startDate.value,Q10_StartDate.value))
        {            
            strErrorMsg="<li>"+campSeasonErrorMessage.value+"</li>";
            bValid = false;
        }
        else if (!CompareDates(Q10_StartDate.value,endDate.value))
        {   
            strErrorMsg="<li>"+campSeasonErrorMessage.value+"</li>";
            bValid = false;
        }
        else if (!CompareDates(startDate.value,Q10_EndDate.value))
        {   
            strErrorMsg="<li>"+campSeasonErrorMessage.value+"</li>";
            bValid = false;
        }
        else if (!CompareDates(Q10_EndDate.value,endDate.value))
        {   
            strErrorMsg="<li>"+campSeasonErrorMessage.value+"</li>";
            bValid = false;
        }
        else
            bValid = true;
    }//end of else if
    else  //Camp registration Question  is not answered
    {
        strErrorMsg="<li>Please answer the Camp registration Question </li>";
        bValid = false;
    }
    
    valObj.innerHTML = strErrorMsg;
    args.IsValid = bValid;
    return;
}

//********************************* END OF VALIDATION FOR Washington DC ***************************
//*****************************BEGIN OF VALIDATION FOR NY**********************************
//to validate the Step2 (Page 2) for NY /////////////////////////
function ValidatePage2Step2_NY(sender,args)
{
    var inputobjs = document.getElementsByTagName("input");
    var selectobjs = document.getElementsByTagName("select");
    var Q3_1, Q3_2, Q4_1, Q4_2, Q5_1, Q5_2, Q6, Q7_1, Q7_2, Q8_1, Q8_2, Q9, Q10;
    Q9 = new Array();
    var j=0;
    var valobj = document.getElementById(sender.id);
    var hdnYearCount;
    for (var i = 0; i< inputobjs.length-1; i++)
    {
        //for Q3_1
        if (inputobjs[i].id.indexOf("RadioBtnQ3_0")>=0)
        {
            Q3_1 = inputobjs[i];
        }
        //for Q3_2
        if (inputobjs[i].id.indexOf("RadioBtnQ3_1")>=0)
        {
            Q3_2 = inputobjs[i];
        }
        
      /*  //for Q4_1
        if (inputobjs[i].id.indexOf("RadioBtnQ4_0")>=0)
        {
            Q4_1 = inputobjs[i];
        }
        
        //for Q4_2
        if (inputobjs[i].id.indexOf("RadioBtnQ4_1")>=0)
        {
            Q4_2 = inputobjs[i];
        }
        
        //for Q5_1
        if (inputobjs[i].id.indexOf("RadioBtnQ5_0")>=0)
        {
            Q5_1 = inputobjs[i];
        }
        
        //for Q5_2
        if (inputobjs[i].id.indexOf("RadioBtnQ5_1")>=0)
        {
            Q5_2 = inputobjs[i];
        }
        */
         //for Q7_1
//        if (inputobjs[i].id.indexOf("RadioBtnQ7_0")>=0)
//        {
//            Q7_1 = inputobjs[i];
//        }
//        
//        //for Q7_2
//        if (inputobjs[i].id.indexOf("RadioBtnQ7_1")>=0)
//        {
//            Q7_2 = inputobjs[i];
//        }  
        
        // for txtSynagogueReferral 
        
//        if (inputobjs[i].id.indexOf("txtSynagogueReferral")>=0)
//        {
//            Q8_2 = inputobjs[i];
//        }      

        //for Q9
        if (inputobjs[i].id.indexOf("RadioBtnQ9")>=0)
        {
            Q9[j] = inputobjs[i];
            j=j+1;
        }      

        //for Q10
        if (inputobjs[i].id.indexOf("txtCamperSchool")>=0)
        {
            Q10 = inputobjs[i];
        }
        
    }  //end of for loop
    
    //to get the <select> objects for Q8 and Q10
    for (var i = 0; i<= selectobjs.length-1; i++)
    {
        //for Q6
         if (selectobjs[i].id.indexOf("ddlGrade")>=0)
        {
            Q6 = selectobjs[i];
        } 
        
        //for Q8
//        if (selectobjs[i].id.indexOf("ddlSynagogue")>=0)
//        {
//            Q8_1 = selectobjs[i];
//        }         

    }

    //validate Q3
    if (Q3_1.checked==false && Q3_2.checked==false)
    {
        valobj.innerHTML = "<ul><li>Please answer Question No. 1</li></ul>";
        args.IsValid=false;
        return;
    }
    var returnVal = ValidateSynagogueAndJCCQuestion(inputobjs,selectobjs,valobj,true);
    if(returnVal == false)
    {
        args.IsValid = false;
        return;
    }
//    else if (Q3_1.checked) // if yes is checked
//    {
//         //for Question 7
//        if (Q7_1.checked==false && Q7_2.checked==false)
//        {
//             valobj.innerHTML = "<li>Please answer Question No. 5</li>";
//             args.IsValid=false;
//             return;
//        }
           //validate synagogue
//        if (Q8_1.selectedIndex==0)
//        {
//            valobj.innerHTML = "<li>Please select the Synagogue</li>";
//            args.IsValid=false;
//            return;
//        }  
        //referral code
//        if (trim(Q8_2.value)=="")
//        {
//            valobj.innerHTML = "<li>Please enter the referral code </li>";
//            args.IsValid=false;
//            return;
//        }  

        
//    }
   /* else if (Q3_2.checked)  //if "no" is checked
    {
        //to validate for Q4 and Q5
        if (Q4_1.checked==false && Q4_2.checked==false)
        {
             valobj.innerHTML = "<li>Please answer Question No. 3</li>";
             args.IsValid=false;
             return;
        }
        else if (Q4_1.checked)
        {
            if (Q5_1.checked==false && Q5_2.checked==false)
            {
                 valobj.innerHTML = "<li>Please answer Question No. 4</li>";
                 args.IsValid=false;
                 return;
            }
            else if (Q5_1.checked)
            {
                //for Question 7
                if (Q7_1.checked==false && Q7_2.checked==false)
                {
                     valobj.innerHTML = "<li>Please answer Question No. 5</li>";
                     args.IsValid=false;
                     return;
                }
                //validate synagogue
                if (Q8_1.selectedIndex==0)
                {
                    valobj.innerHTML = "<li>Please select the Synagogue</li>";
                    args.IsValid=false;
                    return;
                }  
                //referral code
                if (trim(Q8_2.value)=="")
                {
                    valobj.innerHTML = "<li>Please enter the referral code </li>";
                    args.IsValid=false;
                    return;
                }
            }
        }

    }*/
    
    //validate Q6
    if (Q6.selectedIndex==0)
    {
        valobj.innerHTML = "<li>Please select the Grade</li>";
        args.IsValid=false;
        return;
    }   
    
    //for Question 9
    var bQ9Checked = false;
    for (var j =0 ; j<= Q9.length-1; j++)
    {
        if (Q9[j].checked) //Q9 has been answered
        {
            bQ9Checked=true;
            //for Question 11
            if (trim(Q10.value)=="" && Q9[j].value!=3)
            {
                valobj.innerHTML = "<li>Please enter Name of the School</li>";
                args.IsValid=false;
                return;
            }   
        }
    }
    
    // Referral code must be 4 digits
//    if (!(trim(Q8_2.value)==""))
//    {
//        if (!IsNumeric(trim(Q8_2.value)))
//        {
//            valobj.innerHTML = "<ul><li>Please enter valid referal code </li></ul>";
//            args.IsValid=false;
//            return;
// 
//        }   
//    }
    
    // Referral code must be 4 digits
//    if (!(trim(Q8_2.value)==""))
//    {
//        var refcode;
//        refcode=trim(Q8_2.value);
//        if (refcode.length<4)
//        {
//            valobj.innerHTML = "<ul><li>Please enter valid 4 digit referal code </li></ul>";
//            args.IsValid=false;
//            return;
// 
//        }   
//    }


    
    //if Q9 is not answered
    if (!bQ9Checked)
    {
         valobj.innerHTML = "<li>Please answer Question No. 4</li>";
         args.IsValid=false;
         return;
    }  
  
    args.IsValid = true;
    return;
}
//*****************************END OF VALIDATION FOR NY**********************************
//*****************************VALIDATION FOR CHICAGO****************************************

//to validate the Step2 (Page 2) for Greensboro/////////////////////////
function ValidatePage2Step2_Chicago(sender,args)
{
    var inputobjs = document.getElementsByTagName("input");
    var selectobjs = document.getElementsByTagName("select");
    var Q3_1, Q3_2, Q4_1, Q4_2, Q5_1, Q5_2, Q6, Q9, Q10_1, Q10_2, Q11;
    Q9 = new Array();
    var j=0;
    var valobj = document.getElementById(sender.id);
    var hdnYearCount;
    for (var i = 0; i< inputobjs.length-1; i++)
    {
        //for Q3_1
        if (inputobjs[i].id.indexOf("RadioBtnQ3_0")>=0)
        {
            Q3_1 = inputobjs[i];
        }
        //for Q3_2
        if (inputobjs[i].id.indexOf("RadioBtnQ3_1")>=0)
        {
            Q3_2 = inputobjs[i];
        }
        
        //for Q4_1
        if (inputobjs[i].id.indexOf("RadioBtnQ4_0")>=0)
        {
            Q4_1 = inputobjs[i];
        }
        
        //for Q4_2
        if (inputobjs[i].id.indexOf("RadioBtnQ4_1")>=0)
        {
            Q4_2 = inputobjs[i];
        }
        
        //for Q5_1
        if (inputobjs[i].id.indexOf("RadioBtnQ5_0")>=0)
        {
            Q5_1 = inputobjs[i];
        }
        
        //for Q5_2
        if (inputobjs[i].id.indexOf("RadioBtnQ5_1")>=0)
        {
            Q5_2 = inputobjs[i];
        }      
   
        //for Q9
        if (inputobjs[i].id.indexOf("RadioBtnQ9")>=0)
        {
            Q9[j] = inputobjs[i];
            j=j+1;
        }
        
        //for Q10_2
        if (inputobjs[i].id.indexOf("txtJewishSchool")>=0)
        {
            Q10_2 = inputobjs[i];
        }
        
        //for Q11
        if (inputobjs[i].id.indexOf("txtCamperSchool")>=0)
        {
            Q11 = inputobjs[i];
        }
        
    }  //end of for loop
    
    //to get the <select> objects for Q8 and Q10
    for (var i = 0; i<= selectobjs.length-1; i++)
    {
        //for Q6
         if (selectobjs[i].id.indexOf("ddlGrade")>=0)
        {
            Q6 = selectobjs[i];
        }         
      
        //for Q10
        if (selectobjs[i].id.indexOf("ddlQ10")>=0)
        {
            Q10_1 = selectobjs[i];
        }
    }

    //validate Q3
    if (Q3_1.checked==false && Q3_2.checked==false)
    {
        valobj.innerHTML = "<ul><li>Please answer Question No. 1</li></ul>";
        args.IsValid=false;
        return;
    }
    else if (Q3_2.checked)  //if "no" is checked
    {
        //to validate for Q4 and Q5
        if (Q4_1.checked==false && Q4_2.checked==false)
        {
             valobj.innerHTML = "<li>Please answer Question No. 2</li>";
             args.IsValid=false;
             return;
        }
        else if (Q4_1.checked)
        {
            if (Q5_1.checked==false && Q5_2.checked==false)
            {
                 valobj.innerHTML = "<li>Please answer Question No. 3</li>";
                 args.IsValid=false;
                 return;
            }
        }

    }    
    //validate Q6
    if (Q6.selectedIndex==0)
    {
        valobj.innerHTML = "<li>Please select the Grade</li>";
        args.IsValid=false;
        return;
    } 
     
    //for Question 9
    var bQ9Checked = false;
    for (var j =0 ; j<= Q9.length-1; j++)
    {
        if (Q9[j].checked && Q9[j].value==4)
        {
            bQ9Checked = true;
            if (trim(Q10_2.value)=="" && Q10_1.selectedIndex==0)
            {
                 valobj.innerHTML = "<li>Please answer Question No. 6</li>";
                 args.IsValid=false;
                 return;
            }
            else if (trim(Q10_2.value)=="" && Q10_1.options[Q10_1.selectedIndex].text=="Other")  //others is selected
            {
                 valobj.innerHTML = "<li>Please type in the Jewsish Day School</li>";
                 Q10_2.focus();
                 args.IsValid=false;
                 return;
            }
            else if (trim(Q10_2.value)!="")
            {
                for(i=0;i<Q10_1.length;i++)
                {
                    if(Q10_1.options[Q10_1.selectedIndex].text=="Other")
                        Q10_1.selectedIndex=Q10_1.selectedIndex;
                }
            }
        }
        else if (Q9[j].checked) //Q9 has been answered
        {
            bQ9Checked=true;
            //for Question 11
            if (trim(Q11.value)=="" && Q9[j].value!=3)
            {
                valobj.innerHTML = "<li>Please enter Name of the School</li>";
                args.IsValid=false;
                return;
            }   
        }
    }
    
    //if Q9 is not answered
    if (!bQ9Checked)
    {
         valobj.innerHTML = "<li>Please answer Question No. 5</li>";
         args.IsValid=false;
         return;
    }    
 
  
    args.IsValid = true;
    return;
}
//*****************************END OF VALIDATION FOR CHICAGO**********************************

//*****************************VALIDATION FOR RAMAH****************************************
//to validate the Step2 (Page 2) for Ramah/////////////////////////
function ValidatePage2Step2_Ramah(sender,args)
{
    var inputobjs = document.getElementsByTagName("input");
    var selectobjs = document.getElementsByTagName("select");
     
    var Q3_1, Q3_2, Q4_1, Q4_2, Q5_1, Q5_2, Q6, Q9, Q10_1, Q10_2, Q11;
    Q9 = new Array();
    var Q_No_Increment,Q_CampID;
    var j=0;
    var valobj = document.getElementById(sender.id);
    var hdnYearCount;
    for (var i = 0; i< inputobjs.length-1; i++)
    {
        //for finding Question No increment
        if (inputobjs[i].id.indexOf("hdnQNoIncrement")>=0)
        {
            Q_No_Increment = document.getElementById(inputobjs[i].id).value;            
        }
        if (inputobjs[i].id.indexOf("hdnCampId")>=0)
        {
            Q_CampID = document.getElementById(inputobjs[i].id).value;            
        }
        //for Q3_1
        if (inputobjs[i].id.indexOf("RadioBtnQ3_0")>=0)
        {
            Q3_1 = inputobjs[i];
        }
        //for Q3_2
        if (inputobjs[i].id.indexOf("RadioBtnQ3_1")>=0)
        {
            Q3_2 = inputobjs[i];
        }
        
        //for Q4_1
        if (inputobjs[i].id.indexOf("RadioBtnQ4_0")>=0)
        {
            Q4_1 = inputobjs[i];
        }
        
        //for Q4_2
        if (inputobjs[i].id.indexOf("RadioBtnQ4_1")>=0)
        {
            Q4_2 = inputobjs[i];
        }
        
        //for Q5_1
        if (inputobjs[i].id.indexOf("RadioBtnQ5_0")>=0)
        {
            Q5_1 = inputobjs[i];
        }
        
        //for Q5_2
        if (inputobjs[i].id.indexOf("RadioBtnQ5_1")>=0)
        {
            Q5_2 = inputobjs[i];
        }      
   
        //for Q9
        if (inputobjs[i].id.indexOf("RadioBtnQ9")>=0)
        {
            Q9[j] = inputobjs[i];
            j=j+1;
        }
        
        //for Q10_2
      /*  if (inputobjs[i].id.indexOf("txtJewishSchool")>=0)
        {
            Q10_2 = inputobjs[i];
        }
        */
        //for Q11
        if (inputobjs[i].id.indexOf("txtCamperSchool")>=0)
        {
            Q11 = inputobjs[i];
        }
        
    }  //end of for loop
    
    //to get the <select> objects for Q8 and Q10
    for (var i = 0; i<= selectobjs.length-1; i++)
    {
        //for Q6
         if (selectobjs[i].id.indexOf("ddlGrade")>=0)
        {
            Q6 = selectobjs[i];
        }         
      
        //for Q10
       /* if (selectobjs[i].id.indexOf("ddlQ10")>=0)
        {
            Q10_1 = selectobjs[i];
        }*/
    }

    //validate Q3
    if (Q3_1.checked==false && Q3_2.checked==false)
    {
        valobj.innerHTML = "<ul><li>Please answer Question No." +(parseInt(Q_No_Increment)+1) +"</li></ul>";
        args.IsValid=false;
        return;
    }
    else if (Q3_2.checked)  //if "no" is checked
    {
        if(parseInt(Q_CampID) == 3081)
        {
        }
        else
        {
        //to validate for Q4 and Q5
        if (Q4_1.checked==false && Q4_2.checked==false)
        {
             valobj.innerHTML = "<li>Please answer Question No. " +(parseInt(Q_No_Increment)+2) +"</li>";
             args.IsValid=false;
             return;
        }
        else if (Q4_1.checked)
        {
            if (Q5_1.checked==false && Q5_2.checked==false)
            {
                 valobj.innerHTML = "<li>Please answer Question No. " +(parseInt(Q_No_Increment)+3) +"</li>";
                 args.IsValid=false;
                 return;
            }
        }
        }

    }    
    //validate Q6
    if (Q6.selectedIndex==0)
    {
        valobj.innerHTML = "<li>Please select the Grade</li>";
        args.IsValid=false;
        return;
    } 
     
    //for Question 9
    var bQ9Checked = false;
    for (var j =0 ; j<= Q9.length-1; j++)
    {
        /*if (Q9[j].checked && Q9[j].value==4)
        {
            bQ9Checked = true;
            if (trim(Q10_2.value)=="" && Q10_1.selectedIndex==0)
            {
                 valobj.innerHTML = "<li>Please answer Question No. 7</li>";
                 args.IsValid=false;
                 return;
            }
            else if (trim(Q10_2.value)=="" && Q10_1.options[Q10_1.selectedIndex].text=="Other")  //others is selected
            {
                 valobj.innerHTML = "<li>Please type in the Jewsish Day School</li>";
                 Q10_2.focus();
                 args.IsValid=false;
                 return;
            }
            else if (trim(Q10_2.value)!="")
            {
                for(i=0;i<Q10_1.length;i++)
                {
                    if(Q10_1.options[Q10_1.selectedIndex].text=="Other")
                        Q10_1.selectedIndex=Q10_1.selectedIndex;
                }
            }
        }
        else */
        if (Q9[j].checked) //Q9 has been answered
        {
            bQ9Checked=true;
            //for Question 11
            if (trim(Q11.value)=="" && Q9[j].value!=3)
            {
                valobj.innerHTML = "<li>Please enter Name of the School</li>";
                args.IsValid=false;
                return;
            }   
        }
    }
    
    //if Q9 is not answered
    if (!bQ9Checked)
    {
         valobj.innerHTML = "<li>Please answer Question No. " +(parseInt(Q_No_Increment)+5) +"</li>";
         args.IsValid=false;
         return;
    }
  
    args.IsValid = true;
    return;
}

function VaildatePage3Step2_Ramah(sender,args)
{
    var Q7_1, Q7_2, Q7_3, Q7_4, Q8_State, Q8_Camp, Q9_CampSession, Q10_StartDate, Q10_EndDate;
    var inputobjs = document.getElementsByTagName("input");
    var selectobjs = document.getElementsByTagName("select");
    var valObj = document.getElementById(sender.id);
    var dt = new Date();
    var Q_No_Increment;
    var currentDt = dt.getMonth()+ 1 + "/" + dt.getDate() + "/" + dt.getYear();
    var bValid=false;
    var strErrorMsg="";
    var startDate = document.getElementById("ctl00_hdnCampSessionStartDate");
    var endDate = document.getElementById("ctl00_hdnCampSessionEndDate");
    var campSeasonErrorMessage = document.getElementById("ctl00_hdncampSeasonErrorMessage");
    
    for (var i = 0; i<inputobjs.length-1; i++)
    {
        //for finding Question No increment
        if (inputobjs[i].id.indexOf("hdnQNoIncrement")>=0)
        {
            Q_No_Increment = document.getElementById(inputobjs[i].id).value;            
        }
        if (inputobjs[i].id.indexOf("RadioButtonQ7Option1")>=0)
            Q7_1 = inputobjs[i];
        else if (inputobjs[i].id.indexOf("RadioButtonQ7Option2")>=0)
            Q7_2 = inputobjs[i];
//        else if (inputobjs[i].id.indexOf("RadioButtonQ7Option3")>=0)
//            Q7_3 = inputobjs[i];
//        else if (inputobjs[i].id.indexOf("RadioButtonQ7Option4")>=0)
//            Q7_4 = inputobjs[i];
        else if (inputobjs[i].id.indexOf("RadioButtonQ7Option2")>=0)
            Q7_2 = inputobjs[i];
        else if (inputobjs[i].id.indexOf("txtCampSession")>=0)
           Q9_CampSession = inputobjs[i];  
       else if (inputobjs[i].id.indexOf("txtStartDate")>=0)
           Q10_StartDate = inputobjs[i];
        else if (inputobjs[i].id.indexOf("txtEndDate")>=0)
            Q10_EndDate = inputobjs[i];
    } //end of for loop
    
    //to select the dropdown objs
    for (var j=0; j<=selectobjs.length-1; j++)
    {
//        if (selectobjs[j].id.indexOf("ddlState")>=0)
//            Q8_State = selectobjs[j];
//        else if (selectobjs[j].id.indexOf("ddlCampSession")>=0)
//            Q9_CampSession = selectobjs[j];
//        else 
        if (selectobjs[j].id.indexOf("ddlCamp")>=0)
            Q8_Camp = selectobjs[j];
    }
   
    //validation for Question 7
    if (Q7_1.checked)
        bValid=true;
    
    else if (Q7_2.checked )
    {
        Q9_CampSession.value = trim(Q9_CampSession.value);
        Q10_StartDate.value = trim(Q10_StartDate.value);
        Q10_EndDate.value = trim(Q10_EndDate.value);
        //validation for the rest of the questions
        //for Question 10 
        if (Q8_Camp.selectedIndex==0)
        {
            strErrorMsg="<li>Please select a Camp</li>";
            bValid = false;
        }
        else if (Q9_CampSession.value=="") //for Question 11
        {
            strErrorMsg="<li>Please enter a Camp Session</li>";
            bValid = false;
        }
        else if (Q10_StartDate.value=="" || Q10_EndDate.value=="") //for Question 12
        {
            strErrorMsg="<li>Please enter dates in the mm/dd/yyyy format, or select the dates by clicking the calendar icons</li>";
            bValid = false;
        }
        else if (!ValidateDate(Q10_StartDate.value))
        {
            strErrorMsg="<li>Please enter a Valid Start Date in the mm/dd/yyyy format, or select the date by clicking the calendar icon</li>";
            bValid = false;
        }
        else if (!ValidateDate(Q10_EndDate.value))
        {
            strErrorMsg="<li>Please enter a Valid End Date in the mm/dd/yyyy format, or select the date by clicking the calendar icon</li>";
            bValid = false;
        }
        //else if (!CompareDates(currentDt,Q10_StartDate.value))
        //{
        //    strErrorMsg="<li>Start date can not be less than today's date</li>";
        //    bValid = false;
        //}
        else if (!CompareDates(Q10_StartDate.value,Q10_EndDate.value))
        {
            strErrorMsg="<li>Start Date should be less than the End Date</li>";
            bValid = false;
        }
//        //Added by Ram (10/15/2009) related to allow "May, Jun, Jul, Aug, Sep" as session months
//        
        else if (!CompareDates(startDate.value,Q10_StartDate.value))
        {            
            strErrorMsg="<li>"+campSeasonErrorMessage.value+"</li>";
            bValid = false;
        }
        else if (!CompareDates(Q10_StartDate.value,endDate.value))
        {   
            strErrorMsg="<li>"+campSeasonErrorMessage.value+"</li>";
            bValid = false;
        }
        else if (!CompareDates(startDate.value,Q10_EndDate.value))
        {   
            strErrorMsg="<li>"+campSeasonErrorMessage.value+"</li>";
            bValid = false;
        }
        else if (!CompareDates(Q10_EndDate.value,endDate.value))
        {   
            strErrorMsg="<li>"+campSeasonErrorMessage.value+"</li>";
            bValid = false;
        }
        else
            bValid = true;
    }//end of else if
    else  //Camp registration Question  is not answered
    {
        strErrorMsg="<li>Please answer the Camp registration Question </li>";
        bValid = false;
    }
    
    valObj.innerHTML = strErrorMsg;
    args.IsValid = bValid;
    return;
}

//*****************************END OF VALIDATION FOR RAMAH**********************************

//*****************************END OF VALIDATION FOR MetroWest**********************************
function ValidatePage2Step2_MetroWest(sender,args)
{
    var inputobjs = document.getElementsByTagName("input");
    var selectobjs = document.getElementsByTagName("select");
    var Q3_1, Q3_2, Q5, Q6, Q7, Q8, bValid = true, Q7CheckedValue;
    var Q7 = new Array();
    var j=0;
    var valobj = document.getElementById(sender.id);
   
    for (var i = 0; i< inputobjs.length-1; i++)
    {
        //for Q3_1
        if (inputobjs[i].id.indexOf("RadioBtnQ3_0")>=0)
        {
            Q3_1 = inputobjs[i];
        }
        //for Q3_2
        if (inputobjs[i].id.indexOf("RadioBtnQ3_1")>=0)
        {
            Q3_2 = inputobjs[i];
        }
        
        //for school type
        if (inputobjs[i].id.indexOf("RadioBtnQ7")>=0)
        {
            Q7[j] = inputobjs[i];
            j=j+1;
        }     
            
        if (inputobjs[i].id.indexOf("txtCamperSchool")>=0)
            Q8 = inputobjs[i];          
        

    }  //end of for loop   

//    
    //to get the select objects (ddlgrade) for Q6
    for (var k=0; k<= selectobjs.length-1; k++)
    {
        if (selectobjs[k].id.indexOf("ddlGrade")>=0)
        {
            Q6 = selectobjs[k];
            break;
        }        
    }

    //validate Q3
    if (Q3_1.checked==false && Q3_2.checked==false)
    {
        valobj.innerHTML = "<ul><li>Please answer Question No. 1</li></ul>";
        args.IsValid=false;
        return;
        //bValid = false;
    }    
    //For Synagogue and JCC Question
    if (Q3_1.checked==true || Q3_2.checked==true){
        var returnVal = ValidateSynagogueAndJCCQuestion(inputobjs,selectobjs,valobj,false);
        if(returnVal == false)
        {
            args.IsValid = false;
            return;
        }
    }
    
    //validate Q6
    if (Q6.selectedIndex==0)
    {
        valobj.innerHTML = "<ul><li>Please select a Grade</li></ul>";
        args.IsValid=false;
        return;
    }
    else
    {        
        //validate Q7
        var bChecked=false;
         
        for (var k=0; k<=Q7.length-1; k++)
        {
            if (Q7[k].checked==true)
            {
                Q7CheckedValue = Q7[k].value;
                bChecked = true;
                break;
            }
        }
        
        if (!bChecked)
        {
            valobj.innerHTML = "<ul><li>Please answer Question No. 4</li></ul>";
            args.IsValid=false;
            return;
        }
        else if (Q7CheckedValue!="3" && trim(Q8.value)=="") //validate Q8 (if it is not home school)
        {
            valobj.innerHTML = "<ul><li>Please enter Name of the School</li></ul>";
            args.IsValid=false;
            return;
        }
    }   
    args.IsValid = true;
    return;
}
//*****************************END OF VALIDATION FOR MetroWest**********************************


//*****************************START OF VALIDATION FOR DALLAS**********************************

function ValidatePage2Step2_Dallas(sender,args)
{
    var inputobjs = document.getElementsByTagName("input");
    var selectobjs = document.getElementsByTagName("select");
    var Q3_1, Q3_2, Q4_1, Q4_2, Q5_1, Q5_2, Q6, Q8, Q9;
    Q8 = new Array();
    var j=0;
    var valobj = document.getElementById(sender.id);
    var hdnYearCount;
    for (var i = 0; i< inputobjs.length-1; i++)
    {
        //for Q3_1
        if (inputobjs[i].id.indexOf("RadioBtnQ3_0")>=0)
        {
            Q3_1 = inputobjs[i];
        }
        //for Q3_2
        if (inputobjs[i].id.indexOf("RadioBtnQ3_1")>=0)
        {
            Q3_2 = inputobjs[i];
        }
        
        //for Q4_1
        if (inputobjs[i].id.indexOf("RadioBtnQ4_0")>=0)
        {
            Q4_1 = inputobjs[i];
        }
        
        //for Q4_2
        if (inputobjs[i].id.indexOf("RadioBtnQ4_1")>=0)
        {
            Q4_2 = inputobjs[i];
        }
        
        //for Q5_1
        if (inputobjs[i].id.indexOf("RadioBtnQ5_0")>=0)
        {
            Q5_1 = inputobjs[i];
        }
        
        //for Q5_2
        if (inputobjs[i].id.indexOf("RadioBtnQ5_1")>=0)
        {
            Q5_2 = inputobjs[i];
        }      
   
        //for Q8
        if (inputobjs[i].id.indexOf("RadioBtnQ9")>=0)
        {
            Q8[j] = inputobjs[i];
            j=j+1;
        }
                
        //for Q9
        if (inputobjs[i].id.indexOf("txtCamperSchool")>=0)
        {
            Q9 = inputobjs[i];
        }
        
    }  //end of for loop
    
    //to get the <select> objects for Q8 and Q10
    for (var i = 0; i<= selectobjs.length-1; i++)
    {
        //for Q6
         if (selectobjs[i].id.indexOf("ddlGrade")>=0)
        {
            Q6 = selectobjs[i];
        }         
      
    }

    //validate Q3
    if (Q3_1.checked==false && Q3_2.checked==false)
    {
        valobj.innerHTML = "<ul><li>Please answer Question No. 1</li></ul>";
        args.IsValid=false;
        return;
    }
    /*else if (Q3_2.checked)  //if "no" is checked
    {
        //to validate for Q4 and Q5
        if (Q4_1.checked==false && Q4_2.checked==false)
        {
             valobj.innerHTML = "<li>Please answer Question No. 3</li>";
             args.IsValid=false;
             return;
        }
        else if (Q4_1.checked)
        {
            if (Q5_1.checked==false && Q5_2.checked==false)
            {
                 valobj.innerHTML = "<li>Please answer Question No. 4</li>";
                 args.IsValid=false;
                 return;
            }
        }

    }*/   
    //validate Q6
    if (Q6.selectedIndex==0)
    {
        valobj.innerHTML = "<li>Please select the Grade</li>";
        args.IsValid=false;
        return;
    } 
     
     
    //validate Q8
    var bChecked=false;
    var Q8CheckedValue;
    
    for (var k=0; k<=Q8.length-1; k++)
    {
        if (Q8[k].checked==true)
        {
            Q8CheckedValue = Q8[k].value;
            bChecked = true;
            break;
        }
    }
    
    if (!bChecked)
    {
        valobj.innerHTML = "<ul><li>Please answer Question No. 3</li></ul>";
        args.IsValid=false;
        return;
    }
    else if (Q8CheckedValue!="3" && trim(Q9.value)=="")//validate Q6 (if it is not home school)
    {
        valobj.innerHTML = "<ul><li>Please enter Name of the School</li></ul>";
        args.IsValid=false;
        return;
    }

  
    args.IsValid = true;
    return;
}

//*****************************END OF VALIDATION FOR NY**********************************
//*****************************VALIDATION FOR INDIANAPOLLIS****************************************

//to validate the Step2 (Page 2) for Greensboro/////////////////////////
function ValidatePage2Step2_IndianaPollis(sender,args)
{
    var inputobjs = document.getElementsByTagName("input");
    var selectobjs = document.getElementsByTagName("select");
    var Q3_1, Q3_2, Q4_1, Q4_2, Q5_1, Q5_2, Q6, Q9, Q10_1, Q10_2, Q11;
    Q9 = new Array();
    var j=0;
    var valobj = document.getElementById(sender.id);
    var Synagogue_0,Synagogue_1,SynagogueOther;
    var SynagogueComboValue;
    var SynRefCode;
    var hdnYearCount;
    for (var i = 0; i< inputobjs.length-1; i++)
    {
        //for Q3_1
        if (inputobjs[i].id.indexOf("RadioBtnQ3_0")>=0)
        {
            Q3_1 = inputobjs[i];
        }
        //for Q3_2
        if (inputobjs[i].id.indexOf("RadioBtnQ3_1")>=0)
        {
            Q3_2 = inputobjs[i];
        }
        
        //for Q4_1
        if (inputobjs[i].id.indexOf("RadioBtnQ4_0")>=0)
        {
            Q4_1 = inputobjs[i];
        }
        
        //for Q4_2
        if (inputobjs[i].id.indexOf("RadioBtnQ4_1")>=0)
        {
            Q4_2 = inputobjs[i];
        }
        
        //for Q5_1
        if (inputobjs[i].id.indexOf("RadioBtnQ5_0")>=0)
        {
            Q5_1 = inputobjs[i];
        }
        
        //for Q5_2
        if (inputobjs[i].id.indexOf("RadioBtnQ5_1")>=0)
        {
            Q5_2 = inputobjs[i];
        }      
   
        //for Q9
        if (inputobjs[i].id.indexOf("RadioBtnQ9")>=0)
        {
            Q9[j] = inputobjs[i];
            j=j+1;
        }
        
        //for Q10_2
        if (inputobjs[i].id.indexOf("txtJewishSchool")>=0)
        {
            Q10_2 = inputobjs[i];
        }
        
        //for Q11
        if (inputobjs[i].id.indexOf("txtCamperSchool")>=0)
        {
            Q11 = inputobjs[i];
        }
        
    }  //end of for loop
    
    //to get the <select> objects for Q8 and Q10
    for (var i = 0; i<= selectobjs.length-1; i++)
    {
        //for Q6
         if (selectobjs[i].id.indexOf("ddlGrade")>=0)
        {
            Q6 = selectobjs[i];
        }         
      
        //for Q10
        if (selectobjs[i].id.indexOf("ddlQ10")>=0)
        {
            Q10_1 = selectobjs[i];
        }
    }

    //validate Q3
    if (Q3_1.checked==false && Q3_2.checked==false)
    {
        valobj.innerHTML = "<ul><li>Please answer Question No. 1</li></ul>";
        args.IsValid=false;
        return;
    }
     else if (Q3_1.checked) // if yes is checked
    {
            var returnVal = ValidateSynagogueAndJCCQuestion(inputobjs,selectobjs,valobj,false);
            if(returnVal == false)
            {
                args.IsValid = false;
                return;
            }
    }
    else if (Q3_2.checked)  //if "no" is checked
    {
        //to validate for Q4 and Q5
        if (Q4_1.checked==false && Q4_2.checked==false)
        {
             valobj.innerHTML = "<li>Please answer Question No. 2</li>";
             args.IsValid=false;
             return;
        }
        else if (Q4_1.checked) // if "yes" is checked
        {
            if (Q5_1.checked==false && Q5_2.checked==false)
            {
                 valobj.innerHTML = "<li>Please answer Question No. 3</li>";
                 args.IsValid=false;
                 return;
            }
            else if (Q5_1.checked)
            {
                var returnVal = ValidateSynagogueAndJCCQuestion(inputobjs,selectobjs,valobj,false);
                if(returnVal == false)
                {
                    args.IsValid = false;
                    return;
                }
            }
            else if(Q5_2.checked)
            {
                var returnVal = ValidateSynagogueAndJCCQuestion(inputobjs,selectobjs,valobj,false);
                if(returnVal == false)
                {
                    args.IsValid = false;
                    return;
                }
            }
        }
        else if(Q4_2.checked)
        {
            var returnVal = ValidateSynagogueAndJCCQuestion(inputobjs,selectobjs,valobj,false);
            if(returnVal == false)
            {
                args.IsValid = false;
                return;
            }
        }
    }  
   
    //validate Q6
    if (Q6.selectedIndex==0)
    {
        valobj.innerHTML = "<li>Please select the Grade</li>";
        args.IsValid=false;
        return;
    } 
     
    //for Question 9
    var bQ9Checked = false;
    for (var j =0 ; j<= Q9.length-1; j++)
    {
        if (Q9[j].checked) //Q9 has been answered
        {
            bQ9Checked=true;
            //for Question 11
            if (trim(Q11.value)=="")
            {
                valobj.innerHTML = "<li>Please enter Name of the School</li>";
                args.IsValid=false;
                return;
            }   
        }
    }
    
    //if Q9 is not answered
    if (!bQ9Checked)
    {
         valobj.innerHTML = "<li>Please answer Question No. 6</li>";
         args.IsValid=false;
         return;
    }    
 
  
    args.IsValid = true;
    return;
}
//*****************************END OF VALIDATION FOR INDIANAPOLLIS**********************************

// ****************** San Francisco *************************
function ValidatePage2Step2_SanFrancisco_Seattle(sender, args) {
    var inputobjs = document.getElementsByTagName("input"),
        selectobjs = document.getElementsByTagName("select"),
        valobj = document.getElementById(sender.id),
        QFirstTimerYes, QFirstTimerNo, QGrade, QSchoolName;

    for (var i = 0; i < inputobjs.length - 1; i++) {
        if (inputobjs[i].id.indexOf("RadioBtnQ31") >= 0) {
            QFirstTimerYes = inputobjs[i];
        } else if (inputobjs[i].id.indexOf("RadioBtnQ32") >= 0) {
            QFirstTimerNo = inputobjs[i];
        } else if (inputobjs[i].id.indexOf("txtCamperSchool") >= 0) {
            QSchoolName = inputobjs[i];
        }
    }

    for (var i = 0; i <= selectobjs.length - 1; i++) {
        if (selectobjs[i].id.indexOf("ddlGrade") >= 0) {
            QGrade = selectobjs[i];
        }
    }
    
    //validate QFirstTime
    if (QFirstTimerYes.checked == false && QFirstTimerNo.checked == false) {
        valobj.innerHTML = "<ul><li>Please answer Question No. 1</li></ul>";
        args.IsValid = false;
        return;
    }

    //validate Grade
    if (QGrade.selectedIndex == 0) {
        valobj.innerHTML = "<li>Please select the Grade</li>";
        args.IsValid = false;
        return;
    }
    
    //for Question School Name
    if (trim(QSchoolName.value) == "") {
        valobj.innerHTML = "<li>Please enter Name of the School</li>";
        args.IsValid = false;
        return;
    }

    // Check JCC and Synagogue
    var returnVal = ValidateSynagogueAndJCCQuestion(inputobjs, selectobjs, valobj, false);
    if (returnVal == false) {
        args.IsValid = false;
        return;
    }   

    args.IsValid = true;
    return;
}
// ****************** End of San Francisco ******************

//to validate the Step2 (Page 3) of Middlesex Questionaire/////////////////////////
function VaildatePage3Step2_Dallas(sender,args)
{
    var Q7_1, Q7_2, Q7_3, Q7_4, Q8_State, Q8_Camp, Q9_CampSession, Q10_StartDate, Q10_EndDate;
    var inputobjs = document.getElementsByTagName("input");
    var selectobjs = document.getElementsByTagName("select");
    var valObj = document.getElementById(sender.id);
    var dt = new Date();
    var currentDt = dt.getMonth()+ 1 + "/" + dt.getDate() + "/" + dt.getYear();
    var bValid=false;
    var strErrorMsg="";
    
    var startDate = document.getElementById("ctl00_hdnCampSessionStartDate");
    var endDate = document.getElementById("ctl00_hdnCampSessionEndDate");
    var campSeasonErrorMessage = document.getElementById("ctl00_hdncampSeasonErrorMessage");
    
    for (var i = 0; i<inputobjs.length-1; i++)
    {
        if (inputobjs[i].id.indexOf("RadioButtonQ7Option1")>=0)
            Q7_1 = inputobjs[i];
        else if (inputobjs[i].id.indexOf("RadioButtonQ7Option2")>=0)
            Q7_2 = inputobjs[i];
//        else if (inputobjs[i].id.indexOf("RadioButtonQ7Option3")>=0)
//            Q7_3 = inputobjs[i];
//        else if (inputobjs[i].id.indexOf("RadioButtonQ7Option4")>=0)
//            Q7_4 = inputobjs[i];
        else if (inputobjs[i].id.indexOf("RadioButtonQ7Option2")>=0)
            Q7_2 = inputobjs[i];
        else if (inputobjs[i].id.indexOf("txtCampSession")>=0)
            Q9_CampSession = inputobjs[i];  
        else if (inputobjs[i].id.indexOf("txtStartDate")>=0)
            Q10_StartDate = inputobjs[i];
        else if (inputobjs[i].id.indexOf("txtEndDate")>=0)
            Q10_EndDate = inputobjs[i];
    } //end of for loop
    
    //to select the dropdown objs
    for (var j=0; j<=selectobjs.length-1; j++)
    {
        if (selectobjs[j].id.indexOf("ddlState")>=0)
            Q8_State = selectobjs[j];
        else if (selectobjs[j].id.indexOf("ddlCamp")>=0)
            Q8_Camp = selectobjs[j];
    }
   
    //validation for Question 7
    if (Q7_1.checked)
        bValid=true;
    
    else if (Q7_2.checked )
    {
        Q9_CampSession.value = trim(Q9_CampSession.value);
        Q10_StartDate.value = trim(Q10_StartDate.value);
        Q10_EndDate.value = trim(Q10_EndDate.value);
        //validation for the rest of the questions
        //for Question 10 
        if (Q8_Camp.selectedIndex==0)
        {
            strErrorMsg="<li>Please select a Camp</li>";
            bValid = false;
        }
        else if (Q9_CampSession.value=="") //for Question 11
        {
            strErrorMsg="<li>Please enter a Camp Session</li>";
            bValid = false;
        }
        else if (Q10_StartDate.value=="" || Q10_EndDate.value=="") //for Question 12
        {
            strErrorMsg="<li>Please enter dates in the mm/dd/yyyy format, or select the dates by clicking the calendar icons</li>";
            bValid = false;
        }
        else if (!ValidateDate(Q10_StartDate.value))
        {
            strErrorMsg="<li>Please enter a Valid Start Date in the mm/dd/yyyy format, or select the date by clicking the calendar icon</li>";
            bValid = false;
        }
        else if (!ValidateDate(Q10_EndDate.value))
        {
            strErrorMsg="<li>Please enter a Valid End Date in the mm/dd/yyyy format, or select the date by clicking the calendar icon</li>";
            bValid = false;
        }
//        else if (!CompareDates(currentDt,Q10_StartDate.value))
//        {
//            strErrorMsg="<li>Start date can not be less than today's date</li>";
//            bValid = false;
//        }
        else if (!CompareDates(Q10_StartDate.value,Q10_EndDate.value))
        {
            strErrorMsg="<li>Start Date should be less than the End Date</li>";
            bValid = false;
        }
        //Added by Ram (10/15/2009) related to allow "May, Jun, Jul, Aug, Sep" as session months
        
        else if (!CompareDates(startDate.value,Q10_StartDate.value))
        {            
            strErrorMsg="<li>"+campSeasonErrorMessage.value+"</li>";
            bValid = false;
        }
        else if (!CompareDates(Q10_StartDate.value,endDate.value))
        {   
            strErrorMsg="<li>"+campSeasonErrorMessage.value+"</li>";
            bValid = false;
        }
        else if (!CompareDates(startDate.value,Q10_EndDate.value))
        {   
            strErrorMsg="<li>"+campSeasonErrorMessage.value+"</li>";
            bValid = false;
        }
        else if (!CompareDates(Q10_EndDate.value,endDate.value))
        {   
            strErrorMsg="<li>"+campSeasonErrorMessage.value+"</li>";
            bValid = false;
        }
        else
            bValid = true;
    }//end of else if
    else  //Question 9 is not answered
    {
        strErrorMsg="<li>Please answer the Camp registration Question</li>";
        bValid = false;
    }
    
    valObj.innerHTML = strErrorMsg;
    args.IsValid = bValid;
    return;
}

//*****************************END OF VALIDATION FOR DALLAS**********************************
//**************************VALIDATION FOR mountain chai QUESTIONNAIRE****************************

////////////////to validate the Step2 (Page 2) for Middlesex/////////////////////////
function ValidatePage2Step2_MountainChai(sender,args)
{
    var inputobjs = document.getElementsByTagName("input");
    var selectobjs = document.getElementsByTagName("select");
    var Q5, Q7, bValid = true,Q2CheckedValue,Q3CheckedValue,Q4CheckedValue, Q6CheckedValue;
    var Q2 = new Array();
    var Q3 = new Array();
    var Q4 = new Array();
    var Q6 = new Array();
    var j=0;
    var k=0;
    var l=0;
    var m=0;
    var valobj = document.getElementById(sender.id);
    
    for (var i = 0; i< inputobjs.length-1; i++)
    {
        //for Q2
        if (inputobjs[i].id.indexOf("RadioBtnQ2")>=0)
        {    Q2[j] = inputobjs[i]; j=j+1;}
        //for Q3
//        if (inputobjs[i].id.indexOf("RadioBtnQ3")>=0)
//            {Q3[k] = inputobjs[i]; k=k+1;}
//        //for Q4
//        if (inputobjs[i].id.indexOf("RadioBtnQ4")>=0)
//            {Q4[l] = inputobjs[i]; l=l+1;}
        //for Q6
        if (inputobjs[i].id.indexOf("RadioButtionQ6")>=0)
        {
            Q6[m] = inputobjs[i];
            m=m+1;
        }
        //for Q7
        if (inputobjs[i].id.indexOf("txtCamperSchool")>=0)
            Q7 = inputobjs[i];
    }  //end of for loop
    //to get the select objects (ddlgrade) for Q4
    for (var k=0; k<= selectobjs.length-1; k++)
    {
        if (selectobjs[k].id.indexOf("ddlGrade")>=0)
        {
            Q5 = selectobjs[k];
            break;
        }
    }

    //validate Q2
    //validate Q6
    var bQ2Checked=false;
    var bQ3Checked=false;         
    var bQ4Checked=false;
    var bQ6Checked=false;
    for (var k=0; k<=Q2.length-1; k++)
    {
        if (Q2[k].checked==true)
        {
            Q2CheckedValue = Q2[k].value;
            bQ2Checked = true;
            break;
        }
    }    
//    for (var k=0; k<=Q3.length-1; k++)
//    {
//        if (Q3[k].checked==true)
//        {
//            Q3CheckedValue = Q3[k].value;
//            bQ3Checked = true;
//            break;
//        }
//    }
//    for (var k=0; k<=Q4.length-1; k++)
//    {
//        if (Q4[k].checked==true)
//        {
//            Q4CheckedValue = Q4[k].value;
//            bQ4Checked = true;
//            break;
//        }
//    }  
//             
    for (var k=0; k<=Q6.length-1; k++)
    {
        if (Q6[k].checked==true)
        {
            Q6CheckedValue = Q6[k].value;
            bQ6Checked = true;
            break;
        }
    }
    if (!bQ2Checked && bValid)
    {
        valobj.innerHTML = "<ul><li>Please answer Question No. 1</li></ul>";
        bValid = false;
    }
//    else if(Q2CheckedValue == "2")
//    {
//        //validate Q3        
//        if (!bQ3Checked)
//        {
//            valobj.innerHTML = "<ul><li>Please answer Question No. 2</li></ul>";
//            bValid = false;
//        }
//        else if(Q3CheckedValue == "1")
//        {
//            //validate Q4            
//            if (!bQ4Checked)
//            {
//                valobj.innerHTML = "<ul><li>Please answer Question No. 3</li></ul>";
//                bValid = false;
//            }            
//        }        
//    }
    //validate Q5
    if (Q5.selectedIndex==0 && bValid)
    {
        valobj.innerHTML = "<ul><li>Please select a Grade</li></ul>";
        bValid = false;
    }
    
    //validate Q6
    if (!bQ6Checked && bValid)
    {
        valobj.innerHTML = "<ul><li>Please answer Question No. 3</li></ul>";
        bValid = false;
    }
    else if (Q6CheckedValue!="3" && trim(Q7.value)=="" && bValid)//validate Q7(if it is not home school)
    {
        valobj.innerHTML = "<ul><li>Please enter Name of the School</li></ul>";
        bValid = false;
    }
    args.IsValid = bValid;
    return;
}
//End of camp mountain chai
//**************************VALIDATION FOR JCC RANCH QUESTIONNAIRE****************************

////////////////to validate the Step2 (Page 2) for Middlesex/////////////////////////
function ValidatePage2Step2_JCCRanch(sender,args)
{
    var inputobjs = document.getElementsByTagName("input");
    var selectobjs = document.getElementsByTagName("select");
    var Q5, Q7, bValid = true,Q2CheckedValue,Q3CheckedValue,Q4CheckedValue, Q6CheckedValue;
    var Q2 = new Array();
    var Q3 = new Array();
    var Q4 = new Array();
    var Q6 = new Array();
    var j=0;
    var k=0;
    var l=0;
    var m=0;
    var valobj = document.getElementById(sender.id);
    
    for (var i = 0; i< inputobjs.length-1; i++)
    {
        //for Q2
        if (inputobjs[i].id.indexOf("RadioBtnQ2")>=0)
        {    Q2[j] = inputobjs[i]; j=j+1;}
        //for Q3
        if (inputobjs[i].id.indexOf("RadioBtnQ3")>=0)
            {Q3[k] = inputobjs[i]; k=k+1;}
        //for Q4
        if (inputobjs[i].id.indexOf("RadioBtnQ4")>=0)
            {Q4[l] = inputobjs[i]; l=l+1;}
        //for Q6
        if (inputobjs[i].id.indexOf("RadioButtionQ6")>=0)
        {
            Q6[m] = inputobjs[i];
            m=m+1;
        }
        //for Q7
        if (inputobjs[i].id.indexOf("txtCamperSchool")>=0)
            Q7 = inputobjs[i];
    }  //end of for loop
    //to get the select objects (ddlgrade) for Q4
    for (var k=0; k<= selectobjs.length-1; k++)
    {
        if (selectobjs[k].id.indexOf("ddlGrade")>=0)
        {
            Q5 = selectobjs[k];
            break;
        }
    }

    //validate Q2
    //validate Q6
    var bQ2Checked=false;
    var bQ3Checked=false;         
    var bQ4Checked=false;
    var bQ6Checked=false;
    for (var k=0; k<=Q2.length-1; k++)
    {
        if (Q2[k].checked==true)
        {
            Q2CheckedValue = Q2[k].value;
            bQ2Checked = true;
            break;
        }
    }    
    for (var k=0; k<=Q3.length-1; k++)
    {
        if (Q3[k].checked==true)
        {
            Q3CheckedValue = Q3[k].value;
            bQ3Checked = true;
            break;
        }
    }
    for (var k=0; k<=Q4.length-1; k++)
    {
        if (Q4[k].checked==true)
        {
            Q4CheckedValue = Q4[k].value;
            bQ4Checked = true;
            break;
        }
    }  
             
    for (var k=0; k<=Q6.length-1; k++)
    {
        if (Q6[k].checked==true)
        {
            Q6CheckedValue = Q6[k].value;
            bQ6Checked = true;
            break;
        }
    }
    if (!bQ2Checked)
    {
        valobj.innerHTML = "<ul><li>Please answer Question No. 1</li></ul>";
        bValid = false;
    }
    else if(Q2CheckedValue == "2")
    {
        // 2012-09-16 JCC Ranch Camp no longer allows second time camper, so original question #2 and #3 is no longer valid.
        // The code below could be removed.
        
        //validate Q3        
//        if (!bQ3Checked)
//        {
//            valobj.innerHTML = "<ul><li>Please answer Question No. 2</li></ul>";
//            bValid = false;
//        }
//        else if(Q3CheckedValue == "1")
//        {
//            //validate Q4            
//            if (!bQ4Checked)
//            {
//                valobj.innerHTML = "<ul><li>Please answer Question No. 3</li></ul>";
//                bValid = false;
//            }            
//        }        
    }
    //validate Q5
    if (Q5.selectedIndex==0 && bValid)
    {
        valobj.innerHTML = "<ul><li>Please select a Grade</li></ul>";
        bValid = false;
    }
    
    //validate Q6
    if (!bQ6Checked && bValid)
    {
        valobj.innerHTML = "<ul><li>Please answer Question No. 5</li></ul>";
        bValid = false;
    }
    else if (Q6CheckedValue!="3" && trim(Q7.value)=="")//validate Q7(if it is not home school)
    {
        valobj.innerHTML = "<ul><li>Please enter Name of the School</li></ul>";
        bValid = false;
    }
    args.IsValid = bValid;
    return;
}
//to validate the Step2 (Page 3) of JCC Ranch Questionaire/////////////////////////
function VaildatePage3Step2_JCCRanch(sender,args)
{
    var Q7_1, Q7_2, Q7_3, Q7_4, Q8_State, Q8_Camp, Q9_CampSession;
    var inputobjs = document.getElementsByTagName("input");
    var selectobjs = document.getElementsByTagName("select");
    var valObj = document.getElementById(sender.id);
    var dt = new Date();
    var currentDt = dt.getMonth()+ 1 + "/" + dt.getDate() + "/" + dt.getYear();
    var bValid=false;
    var strErrorMsg="";    
    
    for (var i = 0; i<inputobjs.length-1; i++)
    {
        if (inputobjs[i].id.indexOf("RadioButtonQ7Option1")>=0)
            Q7_1 = inputobjs[i];
        else if (inputobjs[i].id.indexOf("RadioButtonQ7Option2")>=0)
            Q7_2 = inputobjs[i];
//        else if (inputobjs[i].id.indexOf("RadioButtonQ7Option3")>=0)
//            Q7_3 = inputobjs[i];
//        else if (inputobjs[i].id.indexOf("RadioButtonQ7Option4")>=0)
//            Q7_4 = inputobjs[i];
        else if (inputobjs[i].id.indexOf("RadioButtonQ7Option2")>=0)
            Q7_2 = inputobjs[i];        
    } //end of for loop    
    
    Q8_Camp = selectobjs[0];
    Q9_CampSession = selectobjs[1];
    //validation for Question 7
    if (Q7_1.checked)
        bValid=true;
    
    else if (Q7_2.checked )
    {
        //validation for the rest of the questions
        if (Q8_Camp.selectedIndex==0)
        {
            strErrorMsg="<li>Please select a Camp</li>";
            bValid = false;
        }
        else if (Q9_CampSession.selectedIndex==0) //for Question 11
        {
            strErrorMsg="<li>Please select a Camp Session</li>";
            bValid = false;
        }        
        else
            bValid = true;
    }//end of else if
    else  //Question 6 is not answered
    {
        strErrorMsg="<li>Please answer Question No. 7</li>";
        bValid = false;
    }
    
    valObj.innerHTML = strErrorMsg;
    args.IsValid = bValid;
    return;
}
//*************End of VALIDATION FOR JCC RANCH QUESTIONNAIRE ************

//to validate the Step2 (Page 3) of Camp Chi Questionaire/////////////////////////
function VaildatePage3Step2_CampChi(sender,args)
{
    var Q7_1, Q7_2, Q7_3, Q7_4, Q8_State, Q8_Camp, Q9_CampSession
    var inputobjs = document.getElementsByTagName("input");
    var selectobjs = document.getElementsByTagName("select");
    var valObj = document.getElementById(sender.id);
    var bValid=false;
    var strErrorMsg="";
    
    for (var i = 0; i<inputobjs.length-1; i++)
    {
        if (inputobjs[i].id.indexOf("RadioButtonQ7Option1")>=0)
            Q7_1 = inputobjs[i];
        else if (inputobjs[i].id.indexOf("RadioButtonQ7Option2")>=0)
            Q7_2 = inputobjs[i];
//        else if (inputobjs[i].id.indexOf("RadioButtonQ7Option3")>=0)
//            Q7_3 = inputobjs[i];
//        else if (inputobjs[i].id.indexOf("RadioButtonQ7Option4")>=0)
//            Q7_4 = inputobjs[i];
        else if (inputobjs[i].id.indexOf("RadioButtonQ7Option2")>=0)
            Q7_2 = inputobjs[i];        
    } //end of for loop    
    
    Q8_Camp = selectobjs[0];
//    Q9_CampSession = selectobjs[1];
    //validation for Question 7
    if (Q7_1.checked)
        bValid=true;
    
    else if (Q7_2.checked )
    {
        //validation for the rest of the questions
        //for Question 10 
        if (Q8_Camp.selectedIndex==0)
        {
            strErrorMsg="<li>Please select a Camp</li>";
            bValid = false;
        }
        else if (Q9_CampSession.selectedIndex==0) //for Question 11
        {
            strErrorMsg="<li>Please select a Camp Session</li>";
            bValid = false;
        }        
        else
            bValid = true;
    }//end of else if
    else  //Question 7 is not answered
    {
        strErrorMsg="<li>Please answer Question No. 5</li>";
        bValid = false;
    }
    
    valObj.innerHTML = strErrorMsg;
    args.IsValid = bValid;
    return;
}

//**************************VALIDATION FOR CMART MIIP QUESTIONNAIRE****************************

////////////////to validate the Step2 (Page 2) for CMART_MiiP/////////////////////////
function ValidatePage2Step2_CMARTMiiP(sender,args)
{
    var inputobjs = document.getElementsByTagName("input");
    var selectobjs = document.getElementsByTagName("select");
    var Q3_1, Q3_2, Q4,Q8_1,Q8_2,Q9_1,Q9_2, Q5, Q6, bValid = true, Q5CheckedValue, Q7_1, Q7_2;
    var Q5 = new Array();
    var j=0;
    var valobj = document.getElementById(sender.id);
    
    for (var i = 0; i< inputobjs.length-1; i++)
    {
        //for Q3_1
        if (inputobjs[i].id.indexOf("RadioBtnQ3_0")>=0)
        {
            Q3_1 = inputobjs[i];
        }
        //for Q3_2
        if (inputobjs[i].id.indexOf("RadioBtnQ3_1")>=0)
        {
            Q3_2 = inputobjs[i];
        }
             //for Q4_1
        if (inputobjs[i].id.indexOf("RadioBtnQ4_0")>=0)
        {
            Q8_1 = inputobjs[i];
        }
        
        //for Q4_2
        if (inputobjs[i].id.indexOf("RadioBtnQ4_1")>=0)
        {
            Q8_2 = inputobjs[i];
        }
        
        //for Q5_1
        if (inputobjs[i].id.indexOf("RadioBtnQ5_0")>=0)
        {
            Q9_1 = inputobjs[i];
        }
        
        //for Q5_2
        if (inputobjs[i].id.indexOf("RadioBtnQ5_1")>=0)
        {
            Q9_2 = inputobjs[i];
        }
        //for Q5
        if (inputobjs[i].id.indexOf("RadioButtionQ5")>=0)
        {
            Q5[j] = inputobjs[i];
            j=j+1;
        }
        //for Q6
        if (inputobjs[i].id.indexOf("txtCamperSchool")>=0)
            Q6 = inputobjs[i];
            
           //for Q3_1
        if (inputobjs[i].id.indexOf("rdbtnQ71")>=0)
            Q7_1 = inputobjs[i];
        //for Q3_2
        if (inputobjs[i].id.indexOf("rdbtnQ72")>=0)
            Q7_2 = inputobjs[i];
    }  //end of for loop
    
    //to get the select objects (ddlgrade) for Q4
    for (var k=0; k<= selectobjs.length-1; k++)
    {
        if (selectobjs[k].id.indexOf("ddlGrade")>=0)
        {
            Q4 = selectobjs[k];
            break;
        }
    }

    //validate Q3
    if (Q3_1.checked==false && Q3_2.checked==false)
    {
        valobj.innerHTML = "<ul><li>Please answer Question No. 1</li></ul>";
        bValid = false;
    }
    else if (Q3_2.checked)  //if "no" is checked
    {
        //to validate for Q4 and Q5
        if (Q8_1.checked==false && Q8_2.checked==false)
        {
             valobj.innerHTML = "<li>Please answer Question No. 2</li>";
             bValid = false;
        }
        else if (Q8_1.checked)
        {
            if (Q9_1.checked==false && Q9_2.checked==false)
            {
                 valobj.innerHTML = "<li>Please answer Question No. 3</li>";
                 bValid = false;
            }

        }

    }
    //validate Q4
    if(bValid && Q4.selectedIndex==0)
    {
        valobj.innerHTML = "<ul><li>Please select a Grade</li></ul>";
        bValid = false;
    }
//    else
//    {
        //validate Q5
        var bChecked=false;
         
        for (var k=0; k<=Q5.length-1; k++)
        {
            if (Q5[k].checked==true)
            {
                Q5CheckedValue = Q5[k].value;
                bChecked = true;
                break;
            }
        }
        
        if (bValid && !bChecked)
        {
            valobj.innerHTML = "<ul><li>Please answer Question No. 5</li></ul>";
            bValid = false;
        }
        else if (bValid && Q5CheckedValue!="3" && trim(Q6.value)=="")//validate Q6 (if it is not home school)
        {
            valobj.innerHTML = "<ul><li>Please enter Name of the School</li></ul>";
            bValid = false;
        }        
    //}  
    
    //Validate Q7
    if(bValid && Q7_1.checked==false && Q7_2.checked==false)
    {
        valobj.innerHTML = "<ul><li>Please answer Question No. 7</li></ul>";
        bValid = false;
    }

    args.IsValid = bValid;
    return;
}
//*****************************END OF VALIDATION FOR CMART MiiP**********************************


//**************************VALIDATION FOR NAGEELA QUESTIONNAIRE****************************

////////////////to validate the Step2 (Page 2) for Middlesex/////////////////////////
function ValidatePage2Step2_Nageela(sender,args)
{
    var inputobjs = document.getElementsByTagName("input");
    var selectobjs = document.getElementsByTagName("select");
    var Q5, Q7, bValid = true, Q2CheckedValue,Q3CheckedValue,Q4CheckedValue,Q6CheckedValue;
    var Q6 = new Array();
    var Q2, Q3, Q4;
    Q2 = new Array();Q3 = new Array();Q4 = new Array();
    var j=0;
    var k=0;
    var l=0;
    var m=0;
    
    var valobj = document.getElementById(sender.id);
    
    for (var i = 0; i< inputobjs.length-1; i++)
    {
        if (inputobjs[i].id.indexOf("RadioBtnListQ2")>=0)
        {
            Q2[j] = inputobjs[i];
            j=j+1;
        }
//        if (inputobjs[i].id.indexOf("RadioBtnListQ3")>=0)
//        {
//            Q3[k] = inputobjs[i];
//            k=k+1;
//        }
//        if (inputobjs[i].id.indexOf("RadioBtnListQ4")>=0)
//        {
//            Q4[l] = inputobjs[i];
//            l=l+1;
//        }
        //for Q5
        if (inputobjs[i].id.indexOf("RadioButtionQ6")>=0)
        {
            Q6[m] = inputobjs[i];
            m=m+1;
        }
        //for Q6
        if (inputobjs[i].id.indexOf("txtCamperSchool")>=0)
            Q7 = inputobjs[i];
    }  //end of for loop
    
    //to get the select objects (ddlgrade) for Q4
    for (var k=0; k<= selectobjs.length-1; k++)
    {
        if (selectobjs[k].id.indexOf("ddlGrade")>=0)
        {
            Q5 = selectobjs[k];
            break;
        }
    }
    
    //validate Q3
    var bChecked=false;         
    for (var n=0; n<=Q2.length-1; n++)
    {
        if (Q2[n].checked==true)
        {
            Q2CheckedValue = Q2[n].value;
            bChecked = true;
            break;
        }
    }    
    if (!bChecked)
    {
        valobj.innerHTML = "<ul><li>Please answer Question No. 1</li></ul>";
        bValid = false;
    }
    //validate Q3
//    if(Q2CheckedValue == "2" && bValid)
//    {
//        var bChecked=false;         
//        for (var n=0; n<=Q3.length-1; n++)
//        {
//            if (Q3[n].checked==true)
//            {
//                Q3CheckedValue = Q3[n].value;
//                bChecked = true;
//                break;
//            }
//        }    
//        if (!bChecked)
//        {
//            valobj.innerHTML = "<ul><li>Please answer Question No. 2</li></ul>";
//            bValid = false;
//        }
//    }
//    //validate Q4
//    if(Q3CheckedValue == "1" && bValid)
//    {
//        var bChecked=false;         
//        for (var n=0; n<=Q4.length-1; n++)
//        {
//            if (Q4[n].checked==true)
//            {
//                Q4CheckedValue = Q4[n].value;
//                bChecked = true;
//                break;
//            }
//        }    
//        if (!bChecked)
//        {
//            valobj.innerHTML = "<ul><li>Please answer Question No. 3</li></ul>";
//            bValid = false;
//        }
//    }
    if (bValid && Q5.selectedIndex==0)
    {
        valobj.innerHTML = "<ul><li>Please select a Grade</li></ul>";
        bValid = false;
    }
    //validate Q6
    if(Q5.selectedIndex > 0 && bValid)
    {
        var bChecked=false;
         
        for (var n=0; n<=Q6.length-1; n++)
        {
            if (Q6[n].checked==true)
            {
                Q6CheckedValue = Q6[n].value;
                bChecked = true;
                break;
            }
        }
        
        if (!bChecked)
        {
            valobj.innerHTML = "<ul><li>Please answer Question No. 5</li></ul>";
            bValid = false;
        }
        else if (Q6CheckedValue!="3" && trim(Q7.value)=="")//validate Q6 (if it is not home school)
        {
            valobj.innerHTML = "<ul><li>Please enter Name of the School</li></ul>";
            bValid = false;
        }
    }
    args.IsValid = bValid;
    return;
}


//to validate the Step2 (Page 3) of Nageela Questionaire/////////////////////////
function VaildatePage3Step2_Nageela(sender,args)
{
    var Q7_1, Q7_2, Q7_3, Q7_4, Q8_State, Q8_Camp, Q9_CampSession, Q10_StartDate, Q10_EndDate;
    var inputobjs = document.getElementsByTagName("input");
    var selectobjs = document.getElementsByTagName("select");
    var valObj = document.getElementById(sender.id);
    var dt = new Date();
    var currentDt = dt.getMonth()+ 1 + "/" + dt.getDate() + "/" + dt.getYear();
    var bValid=false;
    var strErrorMsg="";
    var startDate = document.getElementById("ctl00_hdnCampSessionStartDate");
    var endDate = document.getElementById("ctl00_hdnCampSessionEndDate");
    var campSeasonErrorMessage = document.getElementById("ctl00_hdncampSeasonErrorMessage");
    
    for (var i = 0; i<inputobjs.length-1; i++)
    {
        if (inputobjs[i].id.indexOf("RadioButtonQ7Option1")>=0)
            Q7_1 = inputobjs[i];
        else if (inputobjs[i].id.indexOf("RadioButtonQ7Option2")>=0)
            Q7_2 = inputobjs[i];
//        else if (inputobjs[i].id.indexOf("RadioButtonQ7Option3")>=0)
//            Q7_3 = inputobjs[i];
//        else if (inputobjs[i].id.indexOf("RadioButtonQ7Option4")>=0)
//            Q7_4 = inputobjs[i];
        else if (inputobjs[i].id.indexOf("RadioButtonQ7Option2")>=0)
            Q7_2 = inputobjs[i];
        else if (inputobjs[i].id.indexOf("txtCampSession")>=0)
            Q9_CampSession = inputobjs[i];  
        else if (inputobjs[i].id.indexOf("txtStartDate")>=0)
            Q10_StartDate = inputobjs[i];
        else if (inputobjs[i].id.indexOf("txtEndDate")>=0)
            Q10_EndDate = inputobjs[i];
    } //end of for loop
    
    //to select the dropdown objs
    for (var j=0; j<=selectobjs.length-1; j++)
    {
        if (selectobjs[j].id.indexOf("ddlState")>=0)
            Q8_State = selectobjs[j];
        else if (selectobjs[j].id.indexOf("ddlCamp")>=0)
            Q8_Camp = selectobjs[j];
    }
   
    //validation for Question 7
    if (Q7_1.checked)
        bValid=true;
    
    else if (Q7_2.checked )
    {
        Q9_CampSession.value = trim(Q9_CampSession.value);
        Q10_StartDate.value = trim(Q10_StartDate.value);
        Q10_EndDate.value = trim(Q10_EndDate.value);
        //validation for the rest of the questions
        //for Question 10 
        if (Q8_Camp.selectedIndex==0)
        {
            strErrorMsg="<li>Please select a Camp</li>";
            bValid = false;
        }
        else if (Q9_CampSession.value=="") //for Question 11
        {
            strErrorMsg="<li>Please enter a Camp Session</li>";
            bValid = false;
        }
        else if (Q10_StartDate.value=="" || Q10_EndDate.value=="") //for Question 12
        {
            strErrorMsg="<li>Please enter dates in the mm/dd/yyyy format, or select the dates by clicking the calendar icons</li>";
            bValid = false;
        }
        else if (!ValidateDate(Q10_StartDate.value))
        {
            strErrorMsg="<li>Please enter a Valid Start Date in the mm/dd/yyyy format, or select the date by clicking the calendar icon</li>";
            bValid = false;
        }
        else if (!ValidateDate(Q10_EndDate.value))
        {
            strErrorMsg="<li>Please enter a Valid End Date in the mm/dd/yyyy format, or select the date by clicking the calendar icon</li>";
            bValid = false;
        }
        //else if (!CompareDates(currentDt,Q10_StartDate.value))
        //{
        //    strErrorMsg="<li>Start date can not be less than today's date</li>";
        //    bValid = false;
        //}
        else if (!CompareDates(Q10_StartDate.value,Q10_EndDate.value))
        {
            strErrorMsg="<li>Start Date should be less than the End Date</li>";
            bValid = false;
        }
        //Added by Ram (10/15/2009) related to allow "May, Jun, Jul, Aug, Sep" as session months
        
        else if (!CompareDates(startDate.value,Q10_StartDate.value))
        {            
            strErrorMsg="<li>"+campSeasonErrorMessage.value+"</li>";
            bValid = false;
        }
        else if (!CompareDates(Q10_StartDate.value,endDate.value))
        {   
            strErrorMsg="<li>"+campSeasonErrorMessage.value+"</li>";
            bValid = false;
        }
        else if (!CompareDates(startDate.value,Q10_EndDate.value))
        {   
            strErrorMsg="<li>"+campSeasonErrorMessage.value+"</li>";
            bValid = false;
        }
        else if (!CompareDates(Q10_EndDate.value,endDate.value))
        {   
            strErrorMsg="<li>"+campSeasonErrorMessage.value+"</li>";
            bValid = false;
        }
        else
            bValid = true;
    }//end of else if
    else  //Camp registration Question  is not answered
    {
        strErrorMsg="<li>Please answer the Camp registration Question </li>";
        bValid = false;
    }
    
    valObj.innerHTML = strErrorMsg;
    args.IsValid = bValid;
    return;
}

//*****************************END OF VALIDATION FOR NAGEELA**********************************

//**************************VALIDATION FOR KANSAS CITY QUESTIONNAIRE****************************

////////////////to validate the Step2 (Page 2) for Kansas/////////////////////////
function ValidatePage2Step2_Kansas(sender,args)
{
    var inputobjs = document.getElementsByTagName("input");
    var selectobjs = document.getElementsByTagName("select");
    var Q2_1, Q2_2, Q3, Q5_1, Q5_2, Q6, Q7, bValid = true, Q4CheckedValue, Q6CheckedValue;
    var Q4 = new Array();
    var Q6 = new Array();
    var j=0;
    var k=0;
    var valobj = document.getElementById(sender.id);
    
    for (var i = 0; i< inputobjs.length-1; i++)
    {
        //for Q2_1
        if (inputobjs[i].id.indexOf("RadioBtnQ31")>=0)
            Q2_1 = inputobjs[i];
        //for Q2_2
        if (inputobjs[i].id.indexOf("RadioBtnQ32")>=0)
            Q2_2 = inputobjs[i];
        
        //for Q4
//        if (inputobjs[i].id.indexOf("RadioBtnQ4")>=0)
//        {
//            Q4[j] = inputobjs[i];
//            j=j+1;
//        }
        //for Q5_2 (Synagogue TextBox)
//        if (inputobjs[i].id.indexOf("txtOtherSynagogue")>=0)
//            Q5_2 = inputobjs[i];
        //for Q6
        if (inputobjs[i].id.indexOf("RadioButtionQ6")>=0)
        {
            Q6[k] = inputobjs[i];
            k=k+1;
        }
        //for Q7
        if (inputobjs[i].id.indexOf("txtCamperSchool")>=0)
            Q7 = inputobjs[i];
    }  //end of for loop
    
    //to get the select objects (ddlgrade) for Q3
    for (var k=0; k<= selectobjs.length-1; k++)
    {
        if (selectobjs[k].id.indexOf("ddlGrade")>=0)
        {
            Q3 = selectobjs[k];
            break;
        }
    }
    //to get the select objects (ddlSynagogue) for Q3
//    for (var k=0; k<= selectobjs.length-1; k++)
//    {
//        if (selectobjs[k].id.indexOf("ddlSynagogue")>=0)
//        {
//            Q5_1 = selectobjs[k];
//            break;
//        }
//    }

    //validate Q4
//    var bChecked=false;
//         
//    for (var k=0; k<=Q4.length-1; k++)
//    {
//        if (Q4[k].checked==true)
//        {
//            Q4CheckedValue = Q4[k].value;
//            bChecked = true;
//            break;
//        }
//    }

    //validate Q2
    if (Q2_1.checked==false && Q2_2.checked==false)
    {
        valobj.innerHTML = "<ul><li>Please answer Question No. 1</li></ul>";
        bValid = false;
    }
    //validate Q3
    else if (Q3.selectedIndex==0)
    {
        valobj.innerHTML = "<ul><li>Please select a Grade</li></ul>";
        bValid = false;
    }
    //validate Q4
//    else if(!bChecked)
//    {
//        valobj.innerHTML = "<ul><li>Please answer Question No. 4</li></ul>";
//        bValid = false;
//    }
    //validate Q5_1
//    else if (Q4CheckedValue == "1" && Q5_1.selectedIndex==0)
//    {
//        valobj.innerHTML = "<ul><li>Please select a Synagogue.</li></ul>";
//        bValid = false;
//    }
//    //validate Q5_2
//    else if (Q4CheckedValue == "1" && Q5_1.options[Q5_1.selectedIndex].text =="Other" && trim(Q5_2.value)=="")//validate Q5 (if other is selected from synagogue list)
//        {
//            valobj.innerHTML = "<ul><li>Please enter Name of the Synagogue</li></ul>";
//            bValid = false;
//        }
    else
    {
     var returnVal = ValidateSynagogueAndJCCQuestion(inputobjs,selectobjs,valobj,false);
    if(returnVal == false)
    {
        args.IsValid = false;
        return;
    }
        //validate Q6
        var bChecked=false;
         
        for (var k=0; k<=Q6.length-1; k++)
        {
            if (Q6[k].checked==true)
            {
                Q6CheckedValue = Q6[k].value;
                bChecked = true;
                break;
            }
        }
        
        if (!bChecked)
        {
            valobj.innerHTML = "<ul><li>Please answer Question No. 4</li></ul>";
            bValid = false;
        }
        else if (Q6CheckedValue!="3" && trim(Q7.value)=="")//validate Q6 (if it is not home school)
        {
            valobj.innerHTML = "<ul><li>Please enter Name of the School</li></ul>";
            bValid = false;
        }
    }  

    args.IsValid = bValid;
    return;
}
//*****************************END OF VALIDATION FOR KANSAS**********************************
//*****************************START OF VALIDATION FOR Baltimore**********************************

function ValidatePage2Step2_Baltimore(sender,args)
{
    var inputobjs = document.getElementsByTagName("input");
    var selectobjs = document.getElementsByTagName("select");
    var Q2, Q3, Q4, Q5, Q6, Q7, Q8, Q9_a, Q9_b, Q10, Q11_a, Q11_b;
    Q2 = new Array();Q3 = new Array();Q4= new Array();Q6 = new Array();
    Q8 = new Array();Q10 = new Array();
    var j=0;
    var k=0;
    var l=0;
    var m=0;var n=0;var p=0;
    
    var valobj = document.getElementById(sender.id);
    var hdnYearCount;
    for (var i = 0; i< inputobjs.length-1; i++)
    {
        //for Q2
        if (inputobjs[i].id.indexOf("RadioBtnQ2")>=0)
        {
            Q2[j] = inputobjs[i];
            j=j+1;
        }
        //for Q3
        else if (inputobjs[i].id.indexOf("RadioBtnQ3")>=0)
        {
            Q3[k] = inputobjs[i];
            k=k+1;
        }
        
        //for Q4
        else if (inputobjs[i].id.indexOf("RadioBtnQ4")>=0)
        {
            Q4[l] = inputobjs[i];
            l=l+1;
        }           
   
        //for Q6
        else if (inputobjs[i].id.indexOf("RadioBtnQ6")>=0)
        {
            Q6[m] = inputobjs[i];
            m=m+1;
        }
                
        //for Q7
        else if (inputobjs[i].id.indexOf("txtCamperSchool")>=0)
        {
            Q7 = inputobjs[i];
        }
        
        //for Q8
//        else if (inputobjs[i].id.indexOf("RadioBtnQ8")>=0)
//        {
//            Q8[n] = inputobjs[i];
//            n=n+1;
//        } 
        
        //for Q10
        else if (inputobjs[i].id.indexOf("RadioBtnQ10")>=0)
        {
            Q10[p] = inputobjs[i];
            p=p+1;
        }
        
        //for Q11
        else if (inputobjs[i].id.indexOf("txtOtherSynagogueQ11")>=0)
        {
            Q11_b = inputobjs[i];
        }
        
        //for Q9
//        else if (inputobjs[i].id.indexOf("txtOtherSynagogue")>=0)
//        {
//            Q9_b = inputobjs[i];
//        }
        
    }  //end of for loop
    
    for (var i = 0; i<= selectobjs.length-1; i++)
    {
        //for Q5
        if (selectobjs[i].id.indexOf("ddlGrade")>=0)
        {
            Q5 = selectobjs[i];
        } 
        //for Q11_a
        else if (selectobjs[i].id.indexOf("ddlSynagogueQ11")>=0)
        {
            Q11_a = selectobjs[i];
        }         
        //for Q9_a
//        else if (selectobjs[i].id.indexOf("ddlSynagogue")>=0)
//        {
//            Q9_a = selectobjs[i];
//        } 
    }

    var b2Checked=false;
    var b3Checked=false;
    var b4Checked=false;
    
    var Q2CheckedValue;
    var Q3CheckedValue;
    var Q4CheckedValue;
    
    //validate Q2
    for (var k=0; k<=Q2.length-1; k++)
    {
        if (Q2[k].checked==true)
        {
            Q2CheckedValue = Q2[k].value;
            b2Checked = true;
            break;
        }
    }
    
    for (var k=0; k<=Q3.length-1; k++)
    {
        if (Q3[k].checked==true)
        {
            Q3CheckedValue = Q3[k].value;
            b3Checked = true;
            break;
        }
    }
    
    for (var k=0; k<=Q4.length-1; k++)
    {
        if (Q4[k].checked==true)
        {
            Q4CheckedValue = Q4[k].value;
            b4Checked = true;
            break;
        }
    }
    
    if (!b2Checked)
    {
        valobj.innerHTML = "<ul><li>Please answer Question No. 1</li></ul>";
        args.IsValid=false;
        return;
    }
    else if (Q2CheckedValue=="2" && !b3Checked)//validate Q6 (if it is not home school)
    {
    
        valobj.innerHTML = "<ul><li>Please answer Question No. 2</li></ul>";
        args.IsValid=false;
        return;
    }
    else if (Q3CheckedValue=="1" && !b4Checked)//validate Q6 (if it is not home school)
    {
    
        valobj.innerHTML = "<ul><li>Please answer Question No. 3</li></ul>";
        args.IsValid=false;
        return;
    }
      
    //validate Q5
    if (args.IsValid && Q5.selectedIndex==0)
    {
        valobj.innerHTML = "<li>Please select the Grade</li>";
        args.IsValid=false;
        return;
    } 
     
     
    //validate Q6
    var bChecked=false;
    var Q6CheckedValue;
    
    for (var k=0; k<=Q6.length-1; k++)
    {
        if (Q6[k].checked==true)
        {
            Q6CheckedValue = Q6[k].value;
            bChecked = true;
            break;
        }
    }
    
    if (args.IsValid && !bChecked)
    {
        valobj.innerHTML = "<ul><li>Please answer Question No. 5</li></ul>";
        args.IsValid=false;
        return;
    }
    else if (args.IsValid && Q6CheckedValue!="3" && trim(Q7.value)=="")//validate Q6 (if it is not home school)
    {
        valobj.innerHTML = "<ul><li>Please enter Name of the School</li></ul>";
        args.IsValid=false;
        return;
    }
    
    //validate Q8
      var returnVal = ValidateSynagogueAndJCCQuestion(inputobjs,selectobjs,valobj,false);
    if(returnVal == false)
    {
        args.IsValid = false;
        return;
    }
//    bChecked=false;
//    var Q8CheckedValue;
//    
//    for (var k=0; k<=Q8.length-1; k++)
//    {
//        if (Q8[k].checked==true)
//        {
//            Q8CheckedValue = Q8[k].value;
//            bChecked = true;
//            break;
//        }
//    }
    //alert(Q9_a.selectedIndex + "," + Q8CheckedValue +", "+ Q9_a.options[Q9_a.selectedIndex].text.indexOf("Other") + ", "  + Q9_b.value);
    
//    if (args.IsValid && !bChecked)
//    {
//        valobj.innerHTML = "<ul><li>Please answer Question No. 8</li></ul>";
//        args.IsValid=false;
//        return;
//    }
//    else if (args.IsValid && Q8CheckedValue=="1" && Q9_a.selectedIndex==0)//validate Q8 (if synagogue affliated)
//    {
//        valobj.innerHTML = "<ul><li>Please select the Synagogue name.</li></ul>";
//        args.IsValid=false;
//        return;
//    }
//    else if (args.IsValid && Q8CheckedValue=="1" && Q9_a.selectedIndex!=0 && Q9_a.options[Q9_a.selectedIndex].text.indexOf("Other")!= -1 && trim(Q9_b.value)=="")//validate Q8 (if synagogue affliated)
//    {
//        valobj.innerHTML = "<ul><li>Please enter the Synagogue name.</li></ul>";
//        args.IsValid=false;
//        return;
//    }
    
    //validate Q10
    bChecked=false;
    var Q10CheckedValue;
    
    for (var k=0; k<=Q10.length-1; k++)
    {
        if (Q10[k].checked==true)
        {
            Q10CheckedValue = Q10[k].value;
            bChecked = true;
            break;
        }
    }
    //alert(Q11_a.selectedIndex + "," + Q10CheckedValue +", "+ Q11_a.options[Q11_a.selectedIndex].text);
    
    if (args.IsValid && !bChecked)
    {
        valobj.innerHTML = "<ul><li>Please answer Question No. 8</li></ul>";
        args.IsValid=false;
        return;
    }
    else if (args.IsValid && Q10CheckedValue=="1" && Q11_a.selectedIndex==0)//validate Q8 (if synagogue affliated)
    {
        valobj.innerHTML = "<ul><li>Please answer Question No. 9</li></ul>";
        args.IsValid=false;
        return;
    }
    else if (args.IsValid && Q10CheckedValue=="1" && Q11_a.selectedIndex!=0 && Q11_a.options[Q11_a.selectedIndex].text.indexOf("Other")!= -1 && trim(Q11_b.value)=="")//validate Q8 (if synagogue affliated)
    {
        valobj.innerHTML = "<ul><li>Please enter the Synagogue name.</li></ul>";
        args.IsValid=false;
        return;
    }
  
    args.IsValid = true;
    return;
}

//**************************VALIDATION FOR ARKANSAS QUESTIONNAIRE****************************

////////////////to validate the Step2 (Page 2) for Arkansas/////////////////////////
function ValidatePage2Step2_Arkansas(sender,args)
{
    var inputobjs = document.getElementsByTagName("input");
    var selectobjs = document.getElementsByTagName("select");
    var Q2, Q3, Q4, Q5, Q6, Q7, Q8, Q9_a, Q9_b, Q10, Q11_a, Q11_b;
    Q2 = new Array();Q3 = new Array();Q4= new Array();Q6 = new Array();
    var j=0;
    var k=0;
    var l=0;
    var m=0;var n=0;var p=0;
    var valobj = document.getElementById(sender.id);
    var b2Checked=false;
    var b3Checked=false;
    var b4Checked=false;    
    var Q2CheckedValue;
    var Q3CheckedValue;
    var Q4CheckedValue;
    
    for (var i = 0; i< inputobjs.length-1; i++)
    {
        //for Q2
        if (inputobjs[i].id.indexOf("rbtnListQ2")>=0)
        {
            Q2[j] = inputobjs[i];
            j=j+1;
        }
        //for Q3
        else if (inputobjs[i].id.indexOf("rbtnListQ3")>=0)
        {
            Q3[k] = inputobjs[i];
            k=k+1;
        }
        
        //for Q4
        else if (inputobjs[i].id.indexOf("rbtnListQ4")>=0)
        {
            Q4[l] = inputobjs[i];
            l=l+1;
        }          
        //for Q6
        if (inputobjs[i].id.indexOf("RadioButtonQ6")>=0)
        {
            Q6[m] = inputobjs[i];
            m=m+1;
        }
        //for Q7
        if (inputobjs[i].id.indexOf("txtCamperSchool")>=0)
            Q7 = inputobjs[i];
    }  //end of for loop
    
    //to get the select objects (ddlgrade) for Q5
    for (var k=0; k<= selectobjs.length-1; k++)
    {
        if (selectobjs[k].id.indexOf("ddlGrade")>=0)
        {
            Q5 = selectobjs[k];
            break;
        }
    }

    for (var k=0; k<=Q2.length-1; k++)
    {
        if (Q2[k].checked==true)
        {
            Q2CheckedValue = Q2[k].value;
            b2Checked = true;
            break;
        }
    }
    
    for (var k=0; k<=Q3.length-1; k++)
    {
        if (Q3[k].checked==true)
        {
            Q3CheckedValue = Q3[k].value;
            b3Checked = true;
            break;
        }
    }
    
    for (var k=0; k<=Q4.length-1; k++)
    {
        if (Q4[k].checked==true)
        {
            Q4CheckedValue = Q4[k].value;
            b4Checked = true;
            break;
        }
    }
    
    if (!b2Checked)
    {
        valobj.innerHTML = "<ul><li>Please answer Question No. 1</li></ul>";
        args.IsValid=false;
        return;
    }
    else if (Q2CheckedValue=="2" && !b3Checked)//validate Q3
    {
    
        valobj.innerHTML = "<ul><li>Please answer Question No. 2</li></ul>";
        args.IsValid=false;
        return;
    }
    else if (Q3CheckedValue=="1" && !b4Checked)//validate Q4
    {
    
        valobj.innerHTML = "<ul><li>Please answer Question No. 3</li></ul>";
        args.IsValid=false;
        return;
    }
      
    //validate Q5
    if (args.IsValid && Q5.selectedIndex==0)
    {
        valobj.innerHTML = "<li>Please select the Grade</li>";
        args.IsValid=false;
        return;
    } 
     
     
    //validate Q6
    var b6Checked=false;
    var Q6CheckedValue;
    
    for (var k=0; k<=Q6.length-1; k++)
    {
        if (Q6[k].checked==true)
        {
            Q6CheckedValue = Q6[k].value;
            b6Checked = true;
            break;
        }
    }
    if (args.IsValid && !b6Checked)
    {
        valobj.innerHTML = "<ul><li>Please answer Question No. 5</li></ul>";
        args.IsValid=false;
        return;
    }
    else if (args.IsValid && Q6CheckedValue!="3" && trim(Q7.value)=="")//validate Q6 (if it is not home school)
    {
        valobj.innerHTML = "<ul><li>Please enter Name of the School</li></ul>";
        args.IsValid=false;
        return;
    }

    args.IsValid = true;
    return;
}


////////////////to validate the Step2 (Page 2) for PJ Library/////////////////////////
function ValidatePage2Step2_PJLibrary(sender,args)
{
    var inputobjs = document.getElementsByTagName("input");
    var selectobjs = document.getElementsByTagName("select");
    var Q2_1,Q2_2,Q3_1, Q3_2, Q4, Q5, Q6, bValid = true, Q5CheckedValue;
    var Q5 = new Array();
    var j=0;
    var valobj = document.getElementById(sender.id);
    
    for (var i = 0; i< inputobjs.length-1; i++)
    {
        //for Q2_1
        if(inputobjs[i].id.indexOf("txtFirstName1")>=0)
            Q2_1 = inputobjs[i];
            //for Q2_1
        if(inputobjs[i].id.indexOf("txtLastName1")>=0)
            Q2_2 = inputobjs[i];
        //for Q3_1
        if (inputobjs[i].id.indexOf("RadioBtnQ31")>=0)
            Q3_1 = inputobjs[i];
        //for Q3_2
        if (inputobjs[i].id.indexOf("RadioBtnQ32")>=0)
            Q3_2 = inputobjs[i];
        //for Q5
        if (inputobjs[i].id.indexOf("RadioButtionQ5")>=0)
        {
            Q5[j] = inputobjs[i];
            j=j+1;
        }
        //for Q6
        if (inputobjs[i].id.indexOf("txtCamperSchool")>=0)
            Q6 = inputobjs[i];
    }  //end of for loop
    
    //to get the select objects (ddlgrade) for Q4
    for (var k=0; k<= selectobjs.length-1; k++)
    {
        if (selectobjs[k].id.indexOf("ddlGrade")>=0)
        {
            Q4 = selectobjs[k];
            break;
        }
    }

    if(trim(Q2_1.value) == "")
    {
        valobj.innerHTML = "<ul><li>Please enter PJL member first name.</li></ul>";
        bValid = false;
    }
    else if(trim(Q2_2.value) == "")
    {
        valobj.innerHTML = "<ul><li>Please enter PJL member last name.</li></ul>";
        bValid = false;
    }
    //validate Q3
    else if (Q3_1.checked==false && Q3_2.checked==false)
    {
        valobj.innerHTML = "<ul><li>Please answer Question No. 2</li></ul>";
        bValid = false;
    }
    //validate Q4
    else if (Q4.selectedIndex==0)
    {
        valobj.innerHTML = "<ul><li>Please select a Grade</li></ul>";
        bValid = false;
    }
    else
    {
        //validate Q5
        var bChecked=false;
         
        for (var k=0; k<=Q5.length-1; k++)
        {
            if (Q5[k].checked==true)
            {
                Q5CheckedValue = Q5[k].value;
                bChecked = true;
                break;
            }
        }
        
        if (!bChecked)
        {
            valobj.innerHTML = "<ul><li>Please answer Question No. 4</li></ul>";
            bValid = false;
        }
        else if (Q5CheckedValue!="3" && trim(Q6.value)=="")//validate Q6 (if it is not home school)
        {
            valobj.innerHTML = "<ul><li>Please enter Name of the School</li></ul>";
            bValid = false;
        }
    }  

    args.IsValid = bValid;
    return;
}

//to validate the Step2 (Page 3) of Camp Chi Questionaire/////////////////////////
function VaildatePage3Step2_CampAvoda(sender,args)
{
    var Q6_1, Q6_2, Q8_CampSession, Q9_StartDate, Q7_Camp, Q9_EndDate
    var inputobjs = document.getElementsByTagName("input");
    var selectobjs = document.getElementsByTagName("select");
    var valObj = document.getElementById(sender.id);
    var bValid=false;
    var strErrorMsg="";
    var startDate = document.getElementById("ctl00_hdnCampSessionStartDate");
    var endDate = document.getElementById("ctl00_hdnCampSessionEndDate");
    var campSeasonErrorMessage = document.getElementById("ctl00_hdncampSeasonErrorMessage");
    
    for (var i = 0; i<inputobjs.length-1; i++)
    {
        if (inputobjs[i].id.indexOf("RadioButtonQ7Option1")>=0)
            Q6_1 = inputobjs[i];
        else if (inputobjs[i].id.indexOf("RadioButtonQ7Option2")>=0)
            Q6_2 = inputobjs[i];
        else if (inputobjs[i].id.indexOf("txtCampSession")>=0)
            Q8_CampSession = inputobjs[i];     
        else if (inputobjs[i].id.indexOf("txtStartDate")>=0)
            Q9_StartDate = inputobjs[i];
        else if (inputobjs[i].id.indexOf("txtEndDate")>=0)
            Q9_EndDate = inputobjs[i];  
    } //end of for loop    
    
    Q7_Camp = selectobjs[0];
    Q8_CampSession.value = trim(Q8_CampSession.value);
    Q9_StartDate.value = trim(Q9_StartDate.value);
    Q9_EndDate.value = trim(Q9_EndDate.value);
    //validation for Question 7
    if (Q6_1.checked)
        bValid=true;
    
    else if (Q6_2.checked )
    {
        //validation for the rest of the questions
        //for Question 10 
        if (Q7_Camp.selectedIndex==0)
        {
            strErrorMsg="<li>Please select a Camp</li>";
            bValid = false;
        }        
        else if (Q8_CampSession.value=="") //for Question 11
        {
            strErrorMsg="<li>Please enter a Camp Session</li>";
            bValid = false;
        }       
        else if (Q9_StartDate.value=="" || Q9_EndDate.value=="") //for Question 12
        {
            strErrorMsg="<li>Please enter dates in the mm/dd/yyyy format, or select the dates by clicking the calendar icons</li>";
            bValid = false;
        }
        else if (!ValidateDate(Q9_StartDate.value))
        {
            strErrorMsg="<li>Please enter a Valid Start Date in the mm/dd/yyyy format, or select the date by clicking the calendar icon</li>";
            bValid = false;
        }
        else if (!ValidateDate(Q9_EndDate.value))
        {
            strErrorMsg="<li>Please enter a Valid End Date in the mm/dd/yyyy format, or select the date by clicking the calendar icon</li>";
            bValid = false;
        }        
        else if (!CompareDates(Q9_StartDate.value,Q9_EndDate.value))
        {
            strErrorMsg="<li>Start Date should be less than the End Date</li>";
            bValid = false;
        }
        //Added by Ram (10/15/2009) related to allow "May, Jun, Jul, Aug, Sep" as session months
        
        else if (!CompareDates(startDate.value,Q9_StartDate.value))
        {            
            strErrorMsg="<li>"+campSeasonErrorMessage.value+"</li>";
            bValid = false;
        }
        else if (!CompareDates(Q9_StartDate.value,endDate.value))
        {   
            strErrorMsg="<li>"+campSeasonErrorMessage.value+"</li>";
            bValid = false;
        }
        else if (!CompareDates(startDate.value,Q9_EndDate.value))
        {   
            strErrorMsg="<li>"+campSeasonErrorMessage.value+"</li>";
            bValid = false;
        }
        else if (!CompareDates(Q9_EndDate.value,endDate.value))
        {   
            strErrorMsg="<li>"+campSeasonErrorMessage.value+"</li>";
            bValid = false;
        }
        else
            bValid = true;
    }//end of else if
    else  //Question 7 is not answered
    {
        strErrorMsg="<li>Please answer the Camp registration Question.</li>";
        bValid = false;
    }
    
    valObj.innerHTML = strErrorMsg;
    args.IsValid = bValid;
    return;
}

function ValidateForm(isSubmit)
{
    var inputobjs = document.getElementsByTagName("input");
    var spanobjs = document.getElementsByTagName("span");
    var selectobjs = document.getElementsByTagName("select");
    var textareaobjs = document.getElementsByTagName("textarea");
    var QReqType, QtxtCancelComments, QtxtCampSession, QtxtManualStartDate, QtxtManualEndDate, QddlCampSession,QlblSysStartDate, QlblSysEndDate, QlblNewNoOfDays, QlblNewGrant;
    QReqType = new Array();
    var j=0;
    //var valobj = document.getElementById(sender.id);
    var startDate = document.getElementById("hdnCampSessionStartDate");
    var endDate = document.getElementById("hdnCampSessionEndDate");
    var campSeasonErrorMessage = document.getElementById("hdncampSeasonErrorMessage");  
    var valobj = document.getElementById("valobj"); 
    var dateValidationMessage;    
    var lblAdjustmentType;
   
    var bValid = true;
    for (var i = 0; i<= inputobjs.length-1; i++)
    {
        //for request/adjustment type
        if (inputobjs[i].id.indexOf("rdBtnLstAdjustmentType")>=0)
        {
            QReqType[j] = inputobjs[i];
            j=j+1;
        }
        if(inputobjs[i].id.indexOf("txtCampSession") >=0)
            QtxtCampSession = inputobjs[i];
        //for ManualSessionStartDate
        if (inputobjs[i].id.indexOf("txtNewStartDate")>=0)
            QtxtManualStartDate = inputobjs[i];
        //for ManualSessionEndDate
        if (inputobjs[i].id.indexOf("txtNewEndDate")>=0)
            QtxtManualEndDate = inputobjs[i];                
    }  //end of for loop
    
    for (var i = 0; i<= spanobjs.length-1; i++)
    {
        //for SysSessionStartDate
        if (spanobjs[i].id.indexOf("lblSysNewStartDate")>=0)
            QlblSysStartDate = spanobjs[i];
        //for SysSessionEndDate
        if (spanobjs[i].id.indexOf("lblSysNewEndDate")>=0)
            QlblSysEndDate = spanobjs[i]; 
        //for NewCalculatedNoOfDays
        if (spanobjs[i].id.indexOf("lblNewNoOfDays")>=0)
            QlblNewNoOfDays = spanobjs[i]; 
            //for NewCalculateGrantAmount
        if (spanobjs[i].id.indexOf("lblNewGrant")>=0)
            QlblNewGrant = spanobjs[i]; 
        if (spanobjs[i].id.indexOf("lblAdjustmentType")>=0)
            lblAdjustmentType = spanobjs[i];                
    }  //end of for loop
    
    for (var i = 0; i<= textareaobjs.length-1; i++)
    {
        //for CancelComments
        if (textareaobjs[i].id.indexOf("txtCancelComments")>=0)
        {
            QtxtCancelComments = textareaobjs[i];
            break;  
        }              
    }  //end of for loop
    
    //to get the select objects (ddlgrade) for Q5
    for (var k=0; k<= selectobjs.length-1; k++)
    {
        if (selectobjs[k].id.indexOf("ddlCampSession")>=0)
        {
            QddlCampSession = selectobjs[k];
            break;
        }
    }
    
    if(QtxtManualStartDate != null)
        dateValidationMessage = DateFunctions(trim(startDate.value), trim(endDate.value), trim(QtxtManualStartDate.value), trim(QtxtManualEndDate.value),campSeasonErrorMessage);
    
    var QReqTypeCheckedValue,bReqTypeChecked;
    bReqTypeChecked = false;
    
    if(QReqType.length > 0)
    {
        for (var k=0; k<=QReqType.length-1; k++)
        {
            if (QReqType[k].checked==true)
            {
                QReqTypeCheckedValue = QReqType[k].value;
                bReqTypeChecked = true;
                break;
            }
        }
    }    
    else if(lblAdjustmentType.innerText != "")
    {
        if(trim(lblAdjustmentType.innerText) == "Cancellation")
            QReqTypeCheckedValue = "1";
        if(trim(lblAdjustmentType.innerText) == "Session Change")
            QReqTypeCheckedValue = "2";
        bReqTypeChecked = true;
    }    
    
    if(bValid && !bReqTypeChecked)
    {
        valobj.innerHTML = "<ul><li>Please select adjustment type.</li></ul>";  
        self.scrollTo(0,0);
        //args.IsValid=false;
        bValid = false;
        return false;     
    }
    else
    {
        if(bValid && QReqTypeCheckedValue == "1")
        {
            if(trim(QtxtCancelComments.value) == "")
            {
                valobj.innerHTML = "<ul><li>Please enter cancellation reasons.</li></ul>";
                self.scrollTo(0,0);
                //args.IsValid=false;
                bValid = false;
                return false;  
            }
        }
        else if(bValid && QReqTypeCheckedValue == "2")
        {
            if(QddlCampSession != null && QddlCampSession != undefined)
            {
                if(bValid && QddlCampSession.selectedIndex==0)
                {
                    valobj.innerHTML = "<ul><li>Please select a camp session.</li></ul>";
                    self.scrollTo(0,0);
                    //args.IsValid=false;
                    bValid = false;
                    return false;  
                }
                else if(bValid && trim(QlblSysStartDate.innerText) == "")
                {
                    valobj.innerHTML = "<ul><li>Please select a camp session with start date.</li></ul>";
                    self.scrollTo(0,0);
                    //args.IsValid=false;
                    bValid = false;
                    return false; 
                }
                else if(bValid && trim(QlblSysEndDate.innerText) == "")
                {
                    valobj.innerHTML = "<ul><li>Please select a camp session with start date.</li></ul>";
                    self.scrollTo(0,0);
                    //args.IsValid=false;
                    bValid = false;
                    return false; 
                }
            }
            else
            {
                if(bValid && trim(QtxtCampSession.value) == "")
                {
                    valobj.innerHTML = "<ul><li>Please enter camp session name.</li></ul>";
                    self.scrollTo(0,0);
                    //args.IsValid=false;
                    bValid = false;
                    return false; 
                }                
                else if(bValid && dateValidationMessage != "")              
                {
                    valobj.innerHTML = "<ul>"+dateValidationMessage+"</ul>";
                    self.scrollTo(0,0);
                    //args.IsValid=false;
                    bValid = false;
                    return false; 
                }
             }             
            if(bValid && trim(QlblNewNoOfDays.innerText) == "")
            {
                valobj.innerHTML = "<ul><li>Please click calculate button for number of days in the new camp session.</li></ul>";
                self.scrollTo(0,0);
                //args.IsValid=false;
                    bValid = false;
                    return false; 
            }
            if(bValid && trim(QlblNewGrant.innerText) == "")
            {
                valobj.innerHTML = "<ul><li>Please click calculate button for new grant amount.</li></ul>";
                self.scrollTo(0,0);
                //args.IsValid=false;
                    bValid = false;
                    return false; 
            }
        }        
        else 
        {
            valobj.innerHTML = "<ul><li>Please select adjustment type.</li></ul>";  
            self.scrollTo(0,0);
            //args.IsValid=false;
            bValid = false;
            return false; 
            
        }
    }
//    if(bValid && isSubmit == 1 && isSubmitted == 0)
//    {
//        return confirm('Do you wish to continue with the change/cancellation request?');
//    } 
    if(bValid && isSubmit == 1)
    {
        return confirm('Do you wish to continue with the change/cancellation request?');
    }  
}

function jumpScroll() {
   	window.scrollTo(0,0); // horizontal and vertical scroll targets
}


function DateFunctions(startDate, endDate, actualStartDate, actualEndDate,campSeasonErrorMessage)
{
    if (actualStartDate=="" || actualEndDate=="") //for Question 12
    {
        return strErrorMsg="<li>Please enter dates in the mm/dd/yyyy format, or select the dates by clicking the calendar icons</li>";        
    }
    else if (!ValidateDate(actualStartDate))
    {
        return "<li>Please enter a Valid Start Date in the mm/dd/yyyy format, or select the date by clicking the calendar icon</li>";        
    }
    else if (!ValidateDate(actualEndDate))
    {
        return "<li>Please enter a Valid End Date in the mm/dd/yyyy format, or select the date by clicking the calendar icon</li>";
    }
    else if (!CompareDates(actualStartDate,actualEndDate))
    {
        return "<li>Start Date should be less than the End Date</li>";
    }
    //Added by Ram (10/15/2009) related to allow "May, Jun, Jul, Aug, Sep" as session months
    else if (!CompareDates(startDate,actualStartDate))
    {            
        return "<li>"+campSeasonErrorMessage.value+"</li>";
    }
    else if (!CompareDates(actualStartDate,endDate))
    {   
        return "<li>"+campSeasonErrorMessage.value+"</li>";
    }
    else if (!CompareDates(startDate,actualEndDate))
    {   
        return "<li>"+campSeasonErrorMessage.value+"</li>";
    }
    else if (!CompareDates(actualEndDate,endDate))
    {   
        return "<li>"+campSeasonErrorMessage.value+"</li>";
    }
    else
        return "";
}

function setCookie(c_name,value,expireminutes)
{
    var exdate=new Date();
    exdate.setDate(exdate.getMinutes()+expireminutes);
    document.cookie=c_name+ "=" +escape(value)+ ((expireminutes==null) ? "" : ";expires="+exdate.toGMTString());
}
function getCookie(c_name)
{
    if (document.cookie.length>0)
    {
        c_start=document.cookie.indexOf(c_name + "=");
        if (c_start!=-1)
        {
            c_start=c_start + c_name.length+1;
            c_end=document.cookie.indexOf(";",c_start);
            if (c_end==-1) c_end=document.cookie.length;
            return unescape(document.cookie.substring(c_start,c_end));
        }
    }
    return "";
}
function ValidatePJL(sender,args)
{
    var inputobjs = document.getElementsByTagName("input");
    var Q6,strPJL;
    var valobj = document.getElementById(sender.id);
    for (var i = 0; i< inputobjs.length-1; i++)
    {
        if (inputobjs[i].id.indexOf("txtPJL")>=0)
        { 
            Q6 = inputobjs[i];
            break;
        }         
    }     
    strPJL=Q6.value;
    if(Q6!="undefined" && ((trim(strPJL.toUpperCase())=="PJGTC20111A")||(trim(strPJL.toUpperCase())=="PJGTC20111B") ||(trim(strPJL.toUpperCase())=="PJGTC20111C") ||(trim(strPJL.toUpperCase())=="PJGTC20111D") || (trim(strPJL.toUpperCase())=="PJGTC20111E") ||(trim(strPJL.toUpperCase()) == "PJGTC20111R") ||(trim(Q6.value) == "")))
    {
        args.IsValid=true;
        //valobj.innerHTML="<li>Please enter valid PJL Code.</li>";        
    }
    else    
    args.IsValid = false;
}

function openThis(){  
    window.alert('Thank you for beginning an incentive application! \n Please note: Your application is not yet complete, and has not been submitted. Your information will be saved and is available for you to access in the future. We hope you will return soon to complete and submit your application. Thank You!'); 
}  

function ValidateSynagogueAndJCCQuestionForBoston(inputobjs,selectobjs,valobj,refferalCodeRequired)
{
    var Q4_0, Q4_1, Q4_2, Q4_ddlSynagogue, Q4_txtOtherSynagogue, Q4_SynagogueReferral_TextBox, Q4_ddlJCC, Q4_txtJCC;
    var pnlQ8 = document.getElementById("ctl00_Content_pnl8Q");
    for (var k=0; k<= selectobjs.length-1; k++)
    {
        if(selectobjs[k].id=="ctl00_Content_ddlSynagogue")
        {
            Q4_ddlSynagogue= selectobjs[k];
            //break;
        }
        if(selectobjs[k].id.indexOf("ctl00_Content_ddlJCC") >=0)
        {
            Q4_ddlJCC= selectobjs[k];
            //break;
        }
    }
    
    for (var i=0; i<= inputobjs.length-1; i++)
    {
        if(inputobjs[i].type == "checkbox"){
            //for Q4_1
            if (inputobjs[i].id.indexOf("chkNo")>=0)
                Q4_0 = inputobjs[i];
                    
            //for Q4_2
            if (inputobjs[i].id.indexOf("chkSynagogue")>=0)
                Q4_1 = inputobjs[i];
            
            //for Q4_3
            if (inputobjs[i].id.indexOf("chkJCC")>=0)
                Q4_2 = inputobjs[i];
        }
        if (inputobjs[i].id == "ctl00_Content_txtOtherSynagogue")
            Q4_Synagogue_TextBox = inputobjs[i]; 
            
        if (inputobjs[i].id == "ctl00_Content_txtSynagogueReferral")
            Q4_SynagogueReferral_TextBox = inputobjs[i]; 
            
        if (inputobjs[i].id.indexOf("txtJCC")>=0)
            Q4_txtJCC = inputobjs[i]; 
    }
    
    //if(pnlQ8.disabled == false)
    //{
        if(Q4_0.checked == false && Q4_1.checked == false && Q4_2.checked== false)
        {
            valobj.innerHTML = "<ul><li>Please select synagogue/JCC membership.</li></ul>";
            return false;
        }  
        
        if(Q4_0.checked == false)
        {
            if(Q4_1.checked)
            {
                if(Q4_ddlSynagogue != null)
                {
                    if(Q4_ddlSynagogue.selectedIndex == 0)
                    {
                        valobj.innerHTML = "<ul><li>Please select synagogue.</li></ul>";
                        return false;
                    }
                    else if(Q4_ddlSynagogue.options[Q4_ddlSynagogue.selectedIndex].text.indexOf("Other") > -1)
                    {
                        if(trim(Q4_Synagogue_TextBox.value)=="")
                        {
                            valobj.innerHTML = "<ul><li>Please enter name of the synagogue.</li></ul>";
                            return false;
                        }
                    }
                    else if(refferalCodeRequired)
                    {
                        if(trim(Q4_SynagogueReferral_TextBox.value)=="")
                        {
                            valobj.innerHTML = "<ul><li>Please enter Synagogue referal code.</li></ul>";
                            return false;
                        }
                        else if(trim(Q4_SynagogueReferral_TextBox.value)!="" && !IsNumeric(trim(Q4_SynagogueReferral_TextBox.value)) && refferalCodeRequired)
                        {
                            valobj.innerHTML = "<ul><li>Please enter valid referal code.</li></ul>";
                            return false;
                        }  
                    }
                    
                }
                else
                {
                    if(trim(Q4_Synagogue_TextBox.value)=="")
                    {
                        valobj.innerHTML = "<ul><li>Please enter name of the synagogue.</li></ul>";
                        return false;
                    }
                }
            }
            if(Q4_2.checked)
            {
                if(Q4_ddlJCC != null)
                {
                    if(Q4_ddlJCC.selectedIndex == 0)
                    {
                        valobj.innerHTML = "<ul><li>Please select JCC.</li></ul>";
                        return false;
                    }
                    else if(Q4_ddlJCC.options[Q4_ddlJCC.selectedIndex].text.indexOf("Other") > -1 && trim(Q4_txtJCC.value)=="")
                    {
                        valobj.innerHTML = "<ul><li>Please enter name of the JCC.</li></ul>";
                        return false;
                    }
                }
                else
                {
                    if(trim(Q4_txtJCC.value)=="")
                    {
                        valobj.innerHTML = "<ul><li>Please enter name of the JCC.</li></ul>";
                        return false;
                    }
                }
            }
        }
    //}
}

function ValidateSynagogueAndJCCQuestion(inputobjs,selectobjs,valobj,refferalCodeRequired)
{
    var Q4_0, Q4_1, Q4_2, Q4_ddlSynagogue, Q4_txtOtherSynagogue, Q4_SynagogueReferral_TextBox, Q4_ddlJCC, Q4_txtJCC;
    var pnlQ8 = document.getElementById("ctl00_Content_pnl8Q");
    for (var k=0; k<= selectobjs.length-1; k++)
    {
        if(selectobjs[k].id=="ctl00_Content_ddlSynagogue")
        {
            Q4_ddlSynagogue= selectobjs[k];
            //break;
        }
        if(selectobjs[k].id.indexOf("ctl00_Content_ddlJCC") >=0)
        {
            Q4_ddlJCC= selectobjs[k];
            //break;
        }
    }
    
    for (var i=0; i<= inputobjs.length-1; i++)
    {
        if(inputobjs[i].type == "checkbox"){
            //for Q4_1
            if (inputobjs[i].id.indexOf("chkNo")>=0)
                Q4_0 = inputobjs[i];
                    
            //for Q4_2
            if (inputobjs[i].id.indexOf("chkSynagogue")>=0)
                Q4_1 = inputobjs[i];
            
            //for Q4_3
            if (inputobjs[i].id.indexOf("chkJCC")>=0)
                Q4_2 = inputobjs[i];
        }
        if (inputobjs[i].id == "ctl00_Content_txtOtherSynagogue")
            Q4_Synagogue_TextBox = inputobjs[i]; 
            
        if (inputobjs[i].id == "ctl00_Content_txtSynagogueReferral")
            Q4_SynagogueReferral_TextBox = inputobjs[i]; 
            
        if (inputobjs[i].id.indexOf("txtJCC")>=0)
            Q4_txtJCC = inputobjs[i]; 
    }
    
    //if(pnlQ8.disabled == false)
    //{
        if(Q4_0.checked == false && Q4_1.checked == false && Q4_2.checked== false)
        {
            valobj.innerHTML = "<ul><li>Please select synagogue/JCC membership.</li></ul>";
            return false;
        }  
        
        if(Q4_0.checked == false)
        {
            if(Q4_1.checked)
            {
                if(Q4_ddlSynagogue != null)
                {
                    if(Q4_ddlSynagogue.selectedIndex == 0)
                    {
                        valobj.innerHTML = "<ul><li>Please select synagogue.</li></ul>";
                        return false;
                    }
                    else if(Q4_ddlSynagogue.options[Q4_ddlSynagogue.selectedIndex].text.indexOf("Other") > -1)
                    {
                        if(trim(Q4_Synagogue_TextBox.value)=="")
                        {
                            valobj.innerHTML = "<ul><li>Please enter name of the synagogue.</li></ul>";
                            return false;
                        }
                    }
                    else if(refferalCodeRequired)
                    {
                        if(trim(Q4_SynagogueReferral_TextBox.value)=="")
                        {
                            valobj.innerHTML = "<ul><li>Please enter Synagogue referal code.</li></ul>";
                            return false;
                        }
                        else if(trim(Q4_SynagogueReferral_TextBox.value)!="" && !IsNumeric(trim(Q4_SynagogueReferral_TextBox.value)) && refferalCodeRequired)
                        {
                            valobj.innerHTML = "<ul><li>Please enter valid referal code.</li></ul>";
                            return false;
                        }  
                    }
                    
                }
                else
                {
                    if(trim(Q4_Synagogue_TextBox.value)=="")
                    {
                        valobj.innerHTML = "<ul><li>Please enter name of the synagogue.</li></ul>";
                        return false;
                    }
                }
            }
            if(Q4_2.checked)
            {
                if(Q4_ddlJCC != null)
                {
                    if(Q4_ddlJCC.selectedIndex == 0)
                    {
                        valobj.innerHTML = "<ul><li>Please select JCC.</li></ul>";
                        return false;
                    }
                    else if(Q4_ddlJCC.options[Q4_ddlJCC.selectedIndex].text.indexOf("Other") > -1 && trim(Q4_txtJCC.value)=="")
                    {
                        valobj.innerHTML = "<ul><li>Please enter name of the JCC.</li></ul>";
                        return false;
                    }
                }
                else
                {
                    if(trim(Q4_txtJCC.value)=="")
                    {
                        valobj.innerHTML = "<ul><li>Please enter name of the JCC.</li></ul>";
                        return false;
                    }
                }
            }
        }
    //}
}

function ValidateSynagogueAndJCCAndReferralCodeQuestion(inputobjs,selectobjs,valobj)
{    
    var Q4_0, Q4_1, Q4_2, Q4_ddlSynagogue, Q4_txtOtherSynagogue, Q4_ddlJCC, Q4_txtJCC,Q4_SynagogueReferral;
    var pnlQ8 = document.getElementById("ctl00_Content_pnl8Q");
    for (var k=0; k<= selectobjs.length-1; k++)
    {
        if(selectobjs[k].id=="ctl00_Content_ddlSynagogue")
        {
            Q4_ddlSynagogue= selectobjs[k];
            //break;
        }
        if(selectobjs[k].id.indexOf("ctl00_Content_ddlJCC") >=0)
        {
            Q4_ddlJCC= selectobjs[k];
            //break;
        }
    }
    
    for (var i=0; i<= inputobjs.length-1; i++)
    {
        //for Q4_1
        if (inputobjs[i].id.indexOf("chklistQ8_0")>=0)
            Q4_0 = inputobjs[i];
                
        //for Q4_2
        if (inputobjs[i].id.indexOf("chklistQ8_1")>=0)
            Q4_1 = inputobjs[i];
        
        //for Q4_3
        if (inputobjs[i].id.indexOf("chklistQ8_2")>=0)
            Q4_2 = inputobjs[i];
        
        if (inputobjs[i].id == "ctl00_Content_txtOtherSynagogue")
            Q4_Synagogue_TextBox = inputobjs[i]; 
        if (inputobjs[i].id == "ctl00_Content_txtSynagogueReferral")
            Q4_SynagogueReferral_TextBox = inputobjs[i];  
        if (inputobjs[i].id.indexOf("txtJCC")>=0)
            Q4_txtJCC = inputobjs[i]; 
    }
 
    if(pnlQ8.disabled == false)
    {
        if(Q4_0.checked == false && Q4_1.checked == false && Q4_2.checked== false)
        {
            valobj.innerHTML = "<ul><li>Please select synagogue/JCC membership.</li></ul>";
            return false;
        }  
        
        if(Q4_0.checked == false)
        {
            if(Q4_1.checked)
            {
                if(Q4_ddlSynagogue != null)
                {
                    if(Q4_ddlSynagogue.selectedIndex == 0)
                    {
                        valobj.innerHTML = "<ul><li>Please select synagogue.</li></ul>";
                        return false;
                    }
                    else if(Q4_ddlSynagogue.options[Q4_ddlSynagogue.selectedIndex].text.indexOf("Other") > -1 )
                    {
                        if(trim(Q4_Synagogue_TextBox.value)=="")
                        {
                            valobj.innerHTML = "<ul><li>Please enter name of the synagogue.</li></ul>";
                            return false;
                        }
                    }
                    else if(trim(Q4_SynagogueReferral_TextBox.value)=="")
                    {
                        valobj.innerHTML = "<ul><li>Please enter Synagogue referal code </li></ul>";
                        return false;
                    }
                }
                else
                {
                    if(trim(Q4_Synagogue_TextBox.value)=="")
                    {
                        valobj.innerHTML = "<ul><li>Please enter name of the synagogue.</li></ul>";
                        return false;
                    }
                }
            }
            if(Q4_1.checked && !(trim(Q4_SynagogueReferral_TextBox.value)==""))
            {
            
                if (!IsNumeric(trim(Q4_SynagogueReferral_TextBox.value)))
                {
                    valobj.innerHTML = "<ul><li>Please enter valid referal code </li></ul>";
                    return false;
                }  
            }
            if(Q4_2.checked)
            {
                if(Q4_ddlJCC != null)
                {
                    if(Q4_ddlJCC.selectedIndex == 0)
                    {
                        valobj.innerHTML = "<ul><li>Please select JCC.</li></ul>";
                        return false;
                    }
                    else if(Q4_ddlJCC.options[Q4_ddlJCC.selectedIndex].text.indexOf("Other") > -1 && trim(Q4_txtJCC.value)=="")
                    {
                        valobj.innerHTML = "<ul><li>Please enter name of the JCC.</li></ul>";
                        return false;
                    }
                }
                else
                {
                    if(trim(Q4_txtJCC.value)=="")
                    {
                        valobj.innerHTML = "<ul><li>Please enter name of the JCC.</li></ul>";
                        return false;
                    }
                }
            }
        }
    }
}

function CheckSynagogue(chkboxObject)
{
    var tblSynagogue = document.getElementById("ctl00_Content_tblSynagogue");
    var tblJCC = document.getElementById("ctl00_Content_tblJCC");
    var Pnl9a = document.getElementById("ctl00_Content_Pnl9a");
    var Pnl10a = document.getElementById("ctl00_Content_Pnl10a");  
    var chkSynagogue = document.getElementById("ctl00_Content_chkSynagogue");  
    var chkJCC = document.getElementById("ctl00_Content_chkJCC");      
    var chkNo = document.getElementById("ctl00_Content_chkNo");
    var ddlSynagogue = document.getElementById("ctl00_Content_ddlSynagogue");
    var ddlJCC = document.getElementById("ctl00_Content_ddlJCC");
    var txtOtherSynagogue = document.getElementById("ctl00_Content_txtOtherSynagogue");
    var txtJCC = document.getElementById("ctl00_Content_txtJCC");
    var lblOtherSynogogueQues = document.getElementById("ctl00_Content_lblOtherSynogogueQues");
    var lblJCC = document.getElementById("ctl00_Content_lblJCC");
    var lblRefCode = document.getElementById("ctl00_Content_LblRefCode");
    var txtSynagogueReferral = document.getElementById("ctl00_Content_txtSynagogueReferral");
    debugger;
    if(chkboxObject.id.indexOf("chkSynagogue") != -1)
    {
        if(chkboxObject.checked)
        {
            ddlSynagogue.disabled = false;    
            if(ddlSynagogue.options[ddlSynagogue.selectedIndex].text.indexOf("Other") > -1)
            {
                txtOtherSynagogue.disabled = lblOtherSynogogueQues.disabled = false;
            }  
            else if(ddlSynagogue.selectedIndex > 0)
            {                
                if(txtSynagogueReferral != null) {txtSynagogueReferral.disabled = lblRefCode.disabled = false;}
            } 
            else
            {
                txtOtherSynagogue.disabled = lblOtherSynogogueQues.disabled = true;
                if(txtSynagogueReferral != null){txtSynagogueReferral.disabled = lblRefCode.disabled = true;}
            }     
        }
        else
        {
            ddlSynagogue.disabled = txtOtherSynagogue.disabled = lblOtherSynogogueQues.disabled = true;   
            if(txtSynagogueReferral != null){txtSynagogueReferral.disabled = lblRefCode.disabled = true; txtSynagogueReferral.value = ""; }
            if(ddlSynagogue != null) {ddlSynagogue.selectedIndex = 0;}
            txtOtherSynagogue.value = "";             
        }
        chkNo.checked = false;        
    }
    else if(chkboxObject.id.indexOf("chkJCC") != -1){
        if(chkboxObject.checked)
        {
            if(ddlJCC != null)  ddlJCC.disabled = false;
            if(lblJCC == null) {txtJCC.disabled  = false;}
        }
        else
        {
            txtJCC.disabled = true;
            if(ddlJCC != null)  ddlJCC.disabled = true;
            if(lblJCC != null) lblJCC.disabled  = true;
            if(ddlJCC != null) {ddlJCC.selectedIndex = 0;}
            txtJCC.value = "";
        }
        chkNo.checked = false;
    }
    else if(chkboxObject.id.indexOf("chkNo") != -1){  
        chkSynagogue.checked = chkJCC.checked = false;   
        if(ddlSynagogue != null) {ddlSynagogue.selectedIndex = 0;}
        txtOtherSynagogue.value = "";      
        if(txtSynagogueReferral != null) txtSynagogueReferral.value = "";
        if(ddlJCC != null) {ddlJCC.selectedIndex = 0;}
        txtJCC.value = "";  
        var disable = chkboxObject.checked; 
            
        if(chkboxObject.checked)      
        {
            ddlSynagogue.disabled = txtOtherSynagogue.disabled = lblOtherSynogogueQues.disabled = disable;              
            if(txtSynagogueReferral != null) {txtSynagogueReferral.disabled = lblRefCode.disabled;}
            txtJCC.disabled  = disable;
            if(ddlJCC != null)  ddlJCC.disabled = disable;
            if(lblJCC != null) lblJCC.disabled = disable;  
            Pnl9a.disabled = disable;
            Pnl10a.disabled = disable;   
            chkSynagogue.disabled = chkJCC.disabled = disable; 
        }  
        else
        {
            Pnl9a.disabled = disable;
            Pnl10a.disabled = disable;
            chkSynagogue.disabled = chkJCC.disabled = disable; 
        }   
    }
}

function SynagogueJCCDDLChange(ddlObject){  
    var ddlSynagogue = document.getElementById("ctl00_Content_ddlSynagogue");
    var ddlJCC = document.getElementById("ctl00_Content_ddlJCC");
    var txtOtherSynagogue = document.getElementById("ctl00_Content_txtOtherSynagogue");
    var txtJCC = document.getElementById("ctl00_Content_txtJCC");
    var lblOtherSynogogueQues = document.getElementById("ctl00_Content_lblOtherSynogogueQues");
    var lblJCC = document.getElementById("ctl00_Content_lblJCC");
    var lblRefCode = document.getElementById("ctl00_Content_LblRefCode");
    var txtSynagogueReferral = document.getElementById("ctl00_Content_txtSynagogueReferral");
    
    if(ddlObject.id.indexOf("ddlSynagogue") != -1)    
    {
        if(ddlObject.selectedIndex > 1 && ddlObject.options[ddlObject.selectedIndex].text.indexOf("Other") == -1)
        {
            if(txtSynagogueReferral != null) {
                txtSynagogueReferral.disabled = lblRefCode.disabled = false;
            }
        }
        else
        {
            if(txtSynagogueReferral != null) {
                txtSynagogueReferral.disabled = lblRefCode.disabled = true;
                txtSynagogueReferral.value = "";
            }
        }
        if(ddlObject.options[ddlObject.selectedIndex].text.indexOf("Other") > -1)
        {
            txtOtherSynagogue.disabled = lblOtherSynogogueQues.disabled = false;
        }
        else
        {
            txtOtherSynagogue.disabled = lblOtherSynogogueQues.disabled = true;
            txtOtherSynagogue.value = "";            
        }
    }
    else if(ddlObject.id.indexOf("ddlJCC") != -1)
    {
        if(ddlObject.options[ddlObject.selectedIndex].text.indexOf("Other") > -1)
        {
            txtJCC.disabled = lblJCC.disabled = false;
        }
        else
        {
            txtJCC.disabled = lblJCC.disabled = true;
            txtJCC.value = "";
        }
    }
}

function SynagogueReferralJCCDDLChange(ddlObject){  
    var ddlSynagogue = document.getElementById("ctl00_Content_ddlSynagogue");
    var ddlJCC = document.getElementById("ctl00_Content_ddlJCC");
    var txtOtherSynagogue = document.getElementById("ctl00_Content_txtOtherSynagogue");
    var txtJCC = document.getElementById("ctl00_Content_txtJCC");
    var lblOtherSynogogueQues = document.getElementById("ctl00_Content_lblOtherSynogogueQues");
    var lblJCC = document.getElementById("ctl00_Content_lblJCC");
    var txtSynagogueReferral=document.getElementById("ctl00_Content_txtSynagogueReferral");
    var LblRefCode = document.getElementById("ctl00_Content_LblRefCode");
    if(ddlObject.id.indexOf("ddlSynagogue") != -1)    
    {
        if(ddlObject.options[ddlObject.selectedIndex].text.indexOf("Other") > -1)
        {
            txtOtherSynagogue.disabled = lblOtherSynogogueQues.disabled = false;
            txtSynagogueReferral.disabled = true;
            txtSynagogueReferral.value = "";
            LblRefCode.disabled = true;
        }
        else
        {
            txtOtherSynagogue.disabled = lblOtherSynogogueQues.disabled = true;
            txtOtherSynagogue.value = "";
            LblRefCode.disabled = false;
            txtSynagogueReferral.disabled = false;
        }
    }
    else if(ddlObject.id.indexOf("ddlJCC") != -1)
    {
        if(ddlObject.options[ddlObject.selectedIndex].text.indexOf("Other") > -1)
        {
            txtJCC.disabled = lblJCC.disabled = false;
        }
        else
        {
            txtJCC.disabled = lblJCC.disabled = true;
            txtJCC.value = "";
        }
    }
}