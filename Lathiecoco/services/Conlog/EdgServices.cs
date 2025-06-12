using Lathiecoco.models;
using Lathiecoco.models.conlog;
using Lathiecoco.repository.Conlog;
using Lathiecoco.services.Sms;
using Microsoft.VisualBasic;
using System.Net;
using System.Text;
using System.Xml;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Lathiecoco.services.Conlog
{
    public class EdgServices : EDGrep
    {
        private readonly IConfiguration _configuration;

        public EdgServices(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        string dateTime()
        {
            string d = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            d = d.Replace("-", "");
            d = d.Replace(":", "");
            d = d.Replace(" ", "");

            return d;

        }

        string TIMESTAMPE(DateTime date)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan diff = date.ToUniversalTime() - origin;
            return Math.Floor(diff.TotalSeconds).ToString();
        }

        public async Task<ResponseBody<_customer>> confirmCustomer(string numCompteur)
        {
           
            ResponseBody<_customer> rp = new ResponseBody<_customer>();
            _customer cust = new _customer();

            string url = _configuration["EdgParams:url"];
            XmlDocument doc = new XmlDocument();
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);

            request.Method = "POST";
            request.ContentType = "text/xml";
            request.Accept = "application/soap+xml";
            WebHeaderCollection headers = request.Headers;
            headers.Add("SOAPAction", "#POST");
            string postadatat = string.Empty;

            postadatat = @"<?xml version=""1.0"" encoding=""utf-8""?>
                    <soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">
                      <soap:Body>
                        <confirmCustomerReq xmlns=""http://www.nrs.eskom.co.za/xmlvend/revenue/2.1/schema"">
  
	                    <clientID
		                    xmlns=""http://www.nrs.eskom.co.za/xmlvend/base/2.1/schema"" xsi:type=""EANDeviceID"" ean=""" + _configuration["EdgParams:ClientID"] + @""" />
		                    <terminalID
			                    xmlns=""http://www.nrs.eskom.co.za/xmlvend/base/2.1/schema"" xsi:type=""EANDeviceID"" ean="""+ _configuration["EdgParams:TerminalID"] + @""" />
			                    <msgID
				                    xmlns=""http://www.nrs.eskom.co.za/xmlvend/base/2.1/schema"" dateTime=""" + dateTime() + "\" uniqueNumber=\"" + TIMESTAMPE(DateAndTime.Now) + @""" />
				                    <authCred
					                    xmlns=""http://www.nrs.eskom.co.za/xmlvend/base/2.1/schema"">
					                    <opName>"+ _configuration["EdgParams:OperatorID"] + @"</opName>
					                    <password>"+ _configuration["EdgParams:Password"] + @"</password>
				                    </authCred>
				                    <idMethod
					                    xmlns:q1=""http://www.nrs.eskom.co.za/xmlvend/base/2.1/schema""
					                    xmlns=""http://www.nrs.eskom.co.za/xmlvend/revenue/2.1/schema"" xsi:type=""q1:VendIDMethod"">
					                    <q1:meterIdentifier xsi:type=""q1:MeterNumber"" msno=""" + numCompteur + @""" />
				                    </idMethod>
                        </confirmCustomerReq>
                      </soap:Body>
                    </soap:Envelope>
                    ";

           

            try
            {
                byte[] byt = Encoding.UTF8.GetBytes(postadatat);
                request.ContentLength = byt.Length;

                using (var streams = request.GetRequestStream())
                {
                    using (StreamWriter sw = new StreamWriter(streams))
                    {
                        sw.Write(postadatat);
                    }
                }
                HttpWebResponse reponse = (HttpWebResponse)request.GetResponse();
                
                
                if (reponse.StatusCode == HttpStatusCode.OK)
                {
                    StreamReader wr = new StreamReader(reponse.GetResponseStream());
                    string responseInString = wr.ReadToEnd();
                    
                    doc.LoadXml(responseInString);

                    XmlNode root = doc.DocumentElement;
                    XmlNodeList body = root.ChildNodes;
                    XmlNode isExist = body.Item(0).ChildNodes.Item(0);

                    if (isExist.Name.Equals("confirmCustomerResp"))
                    {
                        rp.Code = 200;
                        rp.IsError = false;
                        rp.Msg = "Confirm Customer";

                        // check customer details
                        XmlNode confirmCustResult = isExist.LastChild.ChildNodes.Item(0);
                        //Console.WriteLine(isExist);

                        string customerName = confirmCustResult.Attributes.GetNamedItem("name").Value; 
                        string customerAdress = confirmCustResult.Attributes.GetNamedItem("address").Value;
                        string customerPhoneNumber = confirmCustResult.Attributes.GetNamedItem("contactNo").Value;
                        string[] phoneNumber;

                        XmlNode custVend = isExist.LastChild.ChildNodes.Item(1);
                        XmlNode canVend = custVend.ChildNodes.Item(0);

                        if (customerPhoneNumber != null)
                        {
                            phoneNumber = Strings.Split(customerPhoneNumber, ":");
                           
                            if(phoneNumber.Length >    1 ) {
                                cust.customerPhoneNumber = phoneNumber[1];
                            }
                            else
                            {
                                cust.customerPhoneNumber = phoneNumber[0];
                            }
                            

                        }

                        cust.customerName = customerName;
                        cust.customerAdress = customerAdress;

                        rp.Body = cust;
                    }
                    else if (isExist.Name.Equals("soap:Fault"))
                    {
                        rp.IsError = true;
                        rp.Code = 400;
                        
                        XmlNode XMLVendFaultResp = body.Item(0).ChildNodes.Item(0).ChildNodes.Item(3).ChildNodes.Item(0);
                        string Msgerreur = XMLVendFaultResp.LastChild.InnerText;
                        rp.Msg = Msgerreur;

                       
                    }
                }
            }
            catch (Exception ex)
            {
                rp.Code = 500;
                rp.IsError = true;
                rp.Msg = ex.Message;

            }

            return rp;

        }

        public async Task<ResponseBody<AccountPaymentServicesEdg>> payCustomer(EdgPayment pay)
        {
            ResponseBody<AccountPaymentServicesEdg> rp = new ResponseBody<AccountPaymentServicesEdg>();

            AccountPaymentServicesEdg accP = new AccountPaymentServicesEdg();
            string url = _configuration["EdgParams:url"];
            XmlDocument doc = new XmlDocument();
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            string Currency = "GNF";
            request.Method = "POST";
            request.ContentType = "text/xml";
            request.Accept = "application/soap+xml";
            WebHeaderCollection headers = request.Headers;
            headers.Add("SOAPAction", "#POST");
            string postadatat = string.Empty;

            string dat = dateTime();
            string stampe = TIMESTAMPE(DateTime.Now);

            postadatat = @"<s:Envelope
                 xmlns:s=""http://www.w3.org/2003/05/soap-envelope"">
                 <s:Body
                  xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""
                  xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
                  <creditVendReq
                xmlns=""http://www.nrs.eskom.co.za/xmlvend/revenue/2.1/schema"">
                <clientID xsi:type=""EANDeviceID"" ean=""" + _configuration["EdgParams:ClientID"] + @"""

                    xmlns =""http://www.nrs.eskom.co.za/xmlvend/base/2.1/schema""/>
                 <terminalID xsi:type=""EANDeviceID"" ean=""" + _configuration["EdgParams:TerminalID"] + @"""
                  xmlns=""http://www.nrs.eskom.co.za/xmlvend/base/2.1/schema""/>
                  <msgID dateTime=""" + dat + "\" uniqueNumber=\"" + stampe + @"""
                   xmlns=""http://www.nrs.eskom.co.za/xmlvend/base/2.1/schema""/>
                   <authCred
                    xmlns=""http://www.nrs.eskom.co.za/xmlvend/base/2.1/schema"">
                    <opName>" + _configuration["EdgParams:OperatorID"] + @"</opName>
                    <password>" + _configuration["EdgParams:Password"] + @"</password>
                   </authCred>
                   <idMethod
                    xmlns=""http://www.nrs.eskom.co.za/xmlvend/base/2.1/schema"">
                    <meterIdentifier xsi:type=""MeterNumber"" msno=""" + pay.numCompteur + @"""/>
                   </idMethod>
                   <purchaseValue xsi:type=""PurchaseValueCurrency"">
                    <amt value=""" + pay.montant.ToString() + "\" symbol=\"" + Currency + @"""/>
                   </purchaseValue>
                  </creditVendReq>
                 </s:Body>
                </s:Envelope>";
            
            try
            {
                byte[] byt = Encoding.UTF8.GetBytes(postadatat);
                request.ContentLength = byt.Length;
                using (var streams = request.GetRequestStream())
                {
                    streams.Write(byt, 0, byt.Length);
                }

                HttpWebResponse reponse = (HttpWebResponse)request.GetResponse();
               
                if (reponse.StatusCode == HttpStatusCode.OK)
                {
                    StreamReader wr = new StreamReader(reponse.GetResponseStream());
                    string responseInString = wr.ReadToEnd();
                    doc.LoadXml(responseInString);

                    XmlNode root = doc.DocumentElement;
                    XmlNodeList body = root.ChildNodes;
                    XmlNode isExist = body.Item(0).ChildNodes.Item(0);
                    
                    if (isExist.Name.Equals("creditVendResp"))
                    {
                        rp.IsError = false;
                        rp.Code = 200;
                        rp.Msg = "Credit Purchase Successful";
                        // credit details
                        XmlNode availableCredit = isExist.ChildNodes.Item(9).ChildNodes.Item(0);
                        string balance = availableCredit.Attributes.GetNamedItem("value").Value;
                        string symbol = availableCredit.Attributes.GetNamedItem("symbol").Value;

                        // customer details
                        XmlNode customer = isExist.ChildNodes.Item(11);
                        string customerName = customer.Attributes.GetNamedItem("name").Value;
                        string customerAdress = customer.Attributes.GetNamedItem("address").Value;
                        string customerPhoneNumber = customer.Attributes.GetNamedItem("contactNo").Value;

                        accP.CustomerName = customerName;
                        accP.utilityType = customer.Attributes.GetNamedItem("utilityType").Value;


                        XmlNode creditVend = isExist.ChildNodes.Item(12);
                        accP.reference = creditVend.Attributes.GetNamedItem("receiptNo").Value;
                        XmlNode transactions = creditVend.ChildNodes.Item(0);
                        XmlNodeList tx = doc.GetElementsByTagName("tx");
                        XmlNode creditTokenIssue;
                        XmlNode meterDetails;
                        string unitOfMeasurement = "";
                        string msno = "";
                        XmlNode tokenNode;
                        string token = "";
                        XmlNode unitNode;
                        string units;
                        bool moreThan2 = false;
                        XmlNode amt;
                       
                        foreach (XmlNode t in tx)
                        {
                            if (t.Attributes.GetNamedItem("xsi:type").Value == "CreditVendTx")
                            {
                                amt = t.ChildNodes.Item(0);
                                if ((amt.Name == "amt"))
                                {
                                    creditTokenIssue = t.ChildNodes.Item(1);
                                    string desc = creditTokenIssue.ChildNodes.Item(0).InnerText;
                                    if (desc != "")
                                    {
                                        meterDetails = creditTokenIssue.ChildNodes.Item(1);
                                        unitOfMeasurement = meterDetails.Attributes.GetNamedItem("unitOfMeasurement").Value;
                                        msno = meterDetails.Attributes.GetNamedItem("msno").Value;
                                        tokenNode = creditTokenIssue.ChildNodes.Item(2);
                                        token += tokenNode.InnerText + "|";
                                        unitNode = creditTokenIssue.ChildNodes.Item(3);
                                        units = unitNode.Attributes.GetNamedItem("value").Value;

                                        // paym.amountVend = amount
                                        accP.token = token;
                                        accP.amount = pay.montant;
                                        accP.MeterNumber = msno;
                                        // paym.MeterNumber = msno

                                        accP.EnergyCoast = units;

                                        rp.Body = accP;

                                    }
                                    else
                                    {
                                        token = "Token not generated";
                                        rp.Msg = token;
                                    }
                                        
                                }
                                else
                                {
                                    creditTokenIssue = amt.ChildNodes.Item(2);

                                    token += creditTokenIssue.InnerText + "|";
                                 
                                    rp.Msg = token;
                                }
                            }
                        }
                    }
                    else if (isExist.Name.Equals("soap:Fault"))
                    {
                        XmlNode XMLVendFaultResp = body.Item(0).ChildNodes.Item(0).ChildNodes.Item(3).ChildNodes.Item(0);
                        string Msgerreur = XMLVendFaultResp.LastChild.InnerText;
                        rp.Code = 400;
                        rp.IsError = true;
                        rp.Msg = Msgerreur;
                    }
                }
            }
            catch (Exception ex)
            {

                rp.Code = 500;
                rp.IsError = true;
                rp.Msg = ex.Message;
            }


            return rp;
        }
    }
}
