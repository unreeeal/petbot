function getTitles()
{
  return ["Category",	"Price",	"Currency",	"Date"];
}

function getSheet(prevMonth)
{
  var date=new Date();
  if(prevMonth)
  date.setMonth(date.getMonth()-1);
var sprName=Utilities.formatDate(date, Session.getScriptTimeZone(), "MMM-YY");
var sheet=SpreadsheetApp.getActiveSpreadsheet().getSheetByName(sprName);
if(sheet==null)
{
    sheet=SpreadsheetApp.getActiveSpreadsheet().insertSheet(sprName,1);
    																						
var titles=getTitles();
sheet.getRange(1,1,1,titles.length).setValues([titles]);

}
 
  
return sheet;
  
}




function insertData(name,price,currency)
{
 
  var sheet=getSheet();
  var row=sheet.getLastRow()+1;
  sheet.getRange(row, 1).setValue(name);
  sheet.getRange(row, 2).setValue(price);
  sheet.getRange(row, 3).setValue(currency);
  sheet.getRange(row, 4).setValue(new Date());

  
}


function getDataFromOneMonth(sheet)
{
  var titles=getTitles();
  
   if(sheet.getLastRow()<2)
  return [];
  var db=sheet.getRange(2,1,sheet.getLastRow()-1,titles.length).getValues();
  var res=[];
  
  
  for(var i=0; i<db.length;i++)
  {
    var mod={};
    
    res.push(mod);
    for(var j=0;j< titles.length;j++)
    {
      mod[titles[j]]=db[i][j];
    }
  }

return res;
}

function getData(){
var days=7;
var dataArr=[];
var date=new Date();
if(date.getDay()<=days){

dataArr =(getDataFromOneMonth(getSheet('prev')));

}

 dataArr=dataArr.concat(getDataFromOneMonth(getSheet()));


//Logger.log(JSON.stringify(dataArr));
return dataArr;
}

function doGet(e) {
  var js=JSON.stringify(getData());
return ContentService.createTextOutput(js ).setMimeType(ContentService.MimeType.JSON); 
}

function doPost(e) {
  
  var action= e.parameter.action;
  if(action){
  
  if(action==="add")
  {

  var category= e.parameter.category;
  var price= e.parameter.price;
  var cur='rub';
  if(category && price && cur)
  {
    insertData(category,price,cur);
    var res= 'added '+category+' '+price+cur;
     return ContentService.createTextOutput(res);
  }
  }
  else if( action=="deleteLastRecord")
  {
     return ContentService.createTextOutput(deleteLastRow());
  }
  }
  return ContentService.createTextOutput("nothing");
}