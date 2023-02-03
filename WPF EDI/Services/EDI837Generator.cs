using OopFactory.X12.Parsing.Model.Typed;
using OopFactory.X12.Parsing.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_EDI.Models;

namespace WPF_EDI.Services
{
    public class EDI837Generator
    {
        #region Private Properties

        private Interchange Interchange { get; set; }
        private FunctionGroup FunctionGroup { get; set; }
        private Transaction TransactionSet { get; set; }

        #endregion

        #region Private Fields

        private bool _isProduction = true;
        private int _nHLSegment;
        private int _claimCount;

        private TypedLoopNM1 _submitterLoop1000A;
        private TypedLoopNM1 _receiverLoop1000B;
        private TypedLoopCLM _claim2300Loop;
        private HierarchicalLoop _provider2000AHLoop;
        private HierarchicalLoop _subscriber2000BHLoop;
        private HierarchicalLoop _currentHLLoop;
        private MainWindow _parentWindow;

        #endregion

        public EDI837Generator(MainWindow parent) 
        {
            _parentWindow = parent;
        }

        #region Public Methods

        /// <summary>
        /// Parse the passed X12 file to an xml string.
        /// </summary>
        /// <param name="aX12File"></param>
        /// <returns></returns>

        public string ParseX12FileToXml(string aX12File)
        {
            string xml;

            var parser = new OopFactory.X12.Parsing.X12Parser();
            using (var stream = System.IO.File.OpenRead(aX12File))
            {
                Interchange interchange = parser.ParseMultiple(stream).First();
                xml = interchange.Serialize();
                stream.Close();
            }

            return xml;
        }

        // create method to save file to location selected on Generate EDI screen

        #endregion

        #region Private Methods

        // Creates header section of the document

        private void CreateHeader()
        {
            CreateIdentifier();
            CreateSubmitter();
            CreateReceiver();
        }

        //Creates and added Hierarchical Transaction and Transmission Type Identification segments

        private void CreateIdentifier()
        {
            var lBhtSegment = this.TransactionSet.AddSegment(new TypedSegmentBHT());
            lBhtSegment.BHT01_HierarchicalStructureCode = "0019";                    // Hierarchical Code (Information source, subscriber dependent)
            lBhtSegment.BHT02_TransactionSetPurposeCode = "00";                      // Setting Transaction Set Purpose Code to Original (00)
            lBhtSegment.BHT03_ReferenceIdentification = _parentWindow.clmRefNo.ToString(); // This is where the reference number the user wants to use will be stored in the BHT segment
            lBhtSegment.BHT04_Date = _parentWindow.clmPostDate.SelectedDate;                // Date 'CCYYMMDD' date from date selected from the Claim Post Date Date Picker
            lBhtSegment.BHT06_TransactionTypeCode = "CH";                            // Transaction Type Code 'CH' = Chargeable (at least one billable line)
        }

        //Create Submitter name and EDI segments

        private void CreateSubmitter()
        {
            #region Loop 1000A Submitter Name

            //NM1 Submitter
            _submitterLoop1000A = this.TransactionSet.AddLoop(new TypedLoopNM1("41"));                                //Entity Identifier Code '41' = Submitter
            _submitterLoop1000A.NM102_EntityTypeQualifier = EntityTypeQualifier.NonPersonEntity;                      //Entity Type Qualifier '1' = person '2' = Non-person Entity
            if (string.IsNullOrWhiteSpace(_parentWindow.conLName.ToString()))                                               //Set last name of submitter unless no submitter name exists, then set Facility Name from EDI Options Tab
            {
                _submitterLoop1000A.NM103_NameLastOrOrganizationName = FormatStr(_parentWindow.facName.ToString());
            }
            else
            {
                _submitterLoop1000A.NM103_NameLastOrOrganizationName = FormatStr(_parentWindow.conLName.ToString());
            }
            _submitterLoop1000A.NM108_IdCodeQualifier = "46";                                                        // ID Code Qualifier '46'=Electronic Transmitter Identification Number
            _submitterLoop1000A.NM109_IdCode = FormatAccountNo(_parentWindow.submitterID.ToString());                                    //ID Code pulled from Submitter ID Text Box from the EDI Options Tab

            //Per Submitter EDI Contact Information
            var lContactList = new List<Tuple<CommunicationNumberQualifer, string>>();
            if (!string.IsNullOrWhiteSpace(_parentWindow.conPhoneNo.ToString()) && lContactList.Count < 3)
            {
                lContactList.Add(new Tuple<CommunicationNumberQualifer, string>(CommunicationNumberQualifer.Telephone, FormatPhone(_parentWindow.conPhoneNo.ToString())));
                if (!string.IsNullOrWhiteSpace(_parentWindow.conPhoneExtNo.ToString()))
                {
                    lContactList.Add(new Tuple<CommunicationNumberQualifer, string>(CommunicationNumberQualifer.TelephoneExtension, FormatPhone(_parentWindow.conPhoneExtNo.ToString())));
                }
            }
            else
            {
                lContactList.Add(new Tuple<CommunicationNumberQualifer, string>(CommunicationNumberQualifer.Telephone, FormatPhone(_parentWindow.facPhoneNo.ToString())));
                if (!string.IsNullOrWhiteSpace(_parentWindow.facPhoneExt.ToString()))
                {
                    lContactList.Add(new Tuple<CommunicationNumberQualifer, string>(CommunicationNumberQualifer.TelephoneExtension, FormatPhone(_parentWindow.facPhoneExt.ToString())));
                }
            }

            if (!string.IsNullOrWhiteSpace(_parentWindow.conFaxNo.ToString()) && lContactList.Count < 3)
            {
                lContactList.Add(new Tuple<CommunicationNumberQualifer, string>(CommunicationNumberQualifer.Facsimile, FormatPhone(_parentWindow.conFaxNo.ToString())));
            }

            if (!string.IsNullOrWhiteSpace(_parentWindow.conEmailAdd.ToString()) && lContactList.Count < 3)
            {
                lContactList.Add(new Tuple<CommunicationNumberQualifer, string>(CommunicationNumberQualifer.ElectronicMail, FormatEmail(_parentWindow.conEmailAdd.ToString())));
            }

            var lPerSegment = _submitterLoop1000A.AddSegment(new TypedSegmentPER());
            lPerSegment.PER01_ContactFunctionCode = "IC";                                                          //Contact Function Code 'IC' = Information Contact
            lPerSegment.PER02_Name = FormatStr(_parentWindow.conFName + " " + _parentWindow.conLName);                               // Name - Submitter contact name

            if (lContactList.Count > 0)
            {
                lPerSegment.PER03_CommunicationNumberQualifier = lContactList[0].Item1;
                lPerSegment.PER04_CommunicationNumber = lContactList[0].Item2;
            }
            if (lContactList.Count > 1)
            {
                lPerSegment.PER05_CommunicationNumberQualifier = lContactList[1].Item1;
                lPerSegment.PER06_CommunicationNumber = lContactList[1].Item2;
            }
            if (lContactList.Count > 2)
            {
                lPerSegment.PER07_CommunicationNumberQualifier = lContactList[2].Item1;
                lPerSegment.PER08_CommunicationNumber = lContactList[2].Item2;
            }
            #endregion
        }

        //Create receiver segment

        private void CreateReceiver()
        {
            #region Loop 1000B Receiver Name
            _receiverLoop1000B = this.TransactionSet.AddLoop(new TypedLoopNM1("40"));                                 //Entity ID Code '40' = receiver
            _receiverLoop1000B.NM102_EntityTypeQualifier = EntityTypeQualifier.NonPersonEntity;                       //Entity Type Qualifier '1' = person '2' = Non-person Entity
            if (string.IsNullOrWhiteSpace(_parentWindow.conLName.ToString()))                                                  //Set last name of submitter unless no submitter name exists, then set Facility Name from EDI Options Tab
            {
                _receiverLoop1000B.NM103_NameLastOrOrganizationName = FormatStr(_parentWindow.facName.ToString());
            }
            else
            {
                _receiverLoop1000B.NM103_NameLastOrOrganizationName = FormatStr(_parentWindow.conLName.ToString());
            }
            _receiverLoop1000B.NM108_IdCodeQualifier = "46";                                                          //ID Code Qualifier '46' = Electronic Transmitter Identification
            _receiverLoop1000B.NM109_IdCode = FormatAccountNo(_parentWindow.receiverID.ToString());                                 //ID Code pulled from receiver ID Text Box from the EDI Options Tab
            #endregion
        }

        //Create Billing Pay to Provider Hierarchical Level

        private void CreateBillingPayToProvider()
        {
            #region Loop 2000A Billing Provider Detail

            //HL - Billing Provider - Hierarchical Level
            _provider2000AHLoop = this.TransactionSet.AddHLoop("1", "20", true);                                        //HL 1

            //Billing Provider Specialty Information
            if (!string.IsNullOrWhiteSpace(_parentWindow.taxonomyBPTab.ToString()))
            {
                var lPrvSegment = _provider2000AHLoop.AddSegment(new TypedSegmentPRV());                //Specialty Segment
                lPrvSegment.PRV01_ProviderCode = "BI";                                                  //Provider Code - 'BI' = Billing 'PT' =Pay-To
                lPrvSegment.PRV02_ReferenceIdQualifier = "ZZ";                                          //Reference Identification Qualifier 'ZZ'=Mutually Defined
                lPrvSegment.PRV03_ProviderTaxonomyCode = FormatStr(_parentWindow.taxonomyBPTab.ToString());      //Reference Identification Code - Provider Specialty Code
            }

            #region Loop 2010AA Billing Provider Name
            // 2010AA Billing Provider
            var lProvider2010AALoop = _provider2000AHLoop.AddLoop(new TypedLoopNM1("85"));                              //Billing Provider
            lProvider2010AALoop.NM102_EntityTypeQualifier = EntityTypeQualifier.NonPersonEntity;                        //Entity Type Qualifier '1'=person '2'=Non-person entity
            lProvider2010AALoop.NM103_NameLastOrOrganizationName = FormatStr(_parentWindow.nameBPTab.ToString());                //Billing Provider name from Billing Provider tab

            //Use NPI if it is no NULL or empty
            if (!string.IsNullOrWhiteSpace(_parentWindow.npiBPTab.ToString()))
            {
                lProvider2010AALoop.NM108_IdCodeQualifier = PhysIdentQualifier.HCFA;                                    //ID Code Qualifier 'XX'=used when the employer's ID or the SSN of the provider must be carried in the REF of this loop
                lProvider2010AALoop.NM109_IdCode = FormatAccountNo(_parentWindow.ToString());                           //ID Code - Billing Provider NPI
            }
            else
            {
                lProvider2010AALoop.NM108_IdCodeQualifier = PhysIdentQualifier.employeeNo;                              //ID Code Qualifier 'XX'=used when the employer's ID or the SSN of the provider must be carried in the REF of this loop
                lProvider2010AALoop.NM109_IdCode = FormatAccountNo(_parentWindow.fedtaxBPTab.ToString());                      //Use Fed Tax No if NPI does not exist
            }

            //N3 Billing Provider Address
            //Loop2010AA, Segment N3 is required to have a full street address. No PO box is allowed.
            var lProvider2010AA_N3Segment = lProvider2010AALoop.AddSegment(new TypedSegmentN3());
            lProvider2010AA_N3Segment.N301_AddressInformation = FormatStr(_parentWindow.streetAddBPTab.ToString());              //Address 1
            if (!string.IsNullOrWhiteSpace(_parentWindow.add2BPTab.ToString()))
            {
                lProvider2010AA_N3Segment.N302_AddressInformation = FormatStr(_parentWindow.add2BPTab.ToString());               //Address info on line 2
            }
            var lProvider2010AA_N4Segment = lProvider2010AALoop.AddSegment(new TypedSegmentN4());
            lProvider2010AA_N4Segment.N401_CityName = FormatCity(_parentWindow.cityBPTab.ToString());
            lProvider2010AA_N4Segment.N402_StateOrProvinceCode = FormatStr(_parentWindow.stateBPTab.ToString());
            lProvider2010AA_N4Segment.N403_PostalCode = FormatZip(_parentWindow.zipBPTab.ToString());
            #endregion

            #region Loop 2010AB Pay-To Address Name
            if (!string.IsNullOrWhiteSpace(_parentWindow.payToAddBPTab.ToString()) &&
                 (
                    !FormatStr(_parentWindow.payToAddBPTab.ToString() ?? string.Empty).Trim().Equals(FormatStr(_parentWindow.streetAddBPTab.ToString() ?? string.Empty).Trim(), StringComparison.OrdinalIgnoreCase) ||
                    !FormatStr(_parentWindow.payToAdd2BPTab.ToString() ?? string.Empty).Trim().Equals(FormatStr(_parentWindow.add2BPTab.ToString() ?? string.Empty).Trim(), StringComparison.OrdinalIgnoreCase) ||
                    !FormatCity(_parentWindow.payToCityBPTab.ToString() ?? string.Empty).Trim().Equals(FormatCity(_parentWindow.cityBPTab.ToString() ?? string.Empty).Trim(), StringComparison.OrdinalIgnoreCase) ||
                    !FormatStr(_parentWindow.payToStateBPTab.ToString() ?? string.Empty).Trim().Equals(FormatStr(_parentWindow.stateBPTab.ToString() ?? string.Empty).Trim(), StringComparison.OrdinalIgnoreCase) ||
                    !FormatZip(_parentWindow.payToZipBPTab.ToString() ?? string.Empty).Trim().Equals(FormatZip(_parentWindow.zipBPTab.ToString() ?? string.Empty).Trim(), StringComparison.OrdinalIgnoreCase)
                 )
               )
            {
                var lProvider2010ABLoop = _provider2000AHLoop.AddLoop(new TypedLoopNM1("87"));                          //Billing Provider
                lProvider2010ABLoop.NM102_EntityTypeQualifier = EntityTypeQualifier.NonPersonEntity;                    //Entity Type Qualifier '1' = person '2' = non-person entity

                var lProvider2010AB_N3Segment = lProvider2010ABLoop.AddSegment(new TypedSegmentN3());
                lProvider2010AB_N3Segment.N301_AddressInformation = FormatStr(_parentWindow.payToAddBPTab.ToString());
                if (!string.IsNullOrWhiteSpace(_parentWindow.payToAdd2BPTab.ToString()))
                {
                    lProvider2010AB_N3Segment.N302_AddressInformation = FormatStr(_parentWindow.payToAdd2BPTab.ToString());
                }

                var lProvider2010AB_N4Segment = lProvider2010ABLoop.AddSegment(new TypedSegmentN4());
                lProvider2010AB_N4Segment.N401_CityName = FormatCity(_parentWindow.payToCityBPTab.ToString());
                lProvider2010AB_N4Segment.N402_StateOrProvinceCode = FormatStr(_parentWindow.payToStateBPTab.ToString());
                lProvider2010AB_N4Segment.N403_PostalCode = FormatZip(_parentWindow.payToZipBPTab.ToString());
            }
            #endregion

            #endregion
        }

        private void CreateSubscriberLevel()
        {
            _nHLSegment++;

            _subscriber2000BHLoop = _provider2000AHLoop.AddHLoop(_nHLSegment.ToString(), "22", false);
            var lSegmentSBR = _subscriber2000BHLoop.AddSegment(new TypedSegmentSBR());
        }

        #endregion

        #region Formatting Methods

        private string FormatGender(string aString)
        {
            var lGender = aString.ToUpper().Trim();
            if (lGender == "M" || lGender == "F")
            {
                return lGender;
            }
            else
            {
                return "U";
            }
        }

        private OopFactory.X12.Parsing.Model.Typed.Gender GenderStrToEnum(string aString)
        {
            var lGender = aString.ToUpper().Trim();
            if (lGender == "M")
            {
                return Gender.Male;
            }
            else if (lGender == "F")
            {
                return Gender.Female;
            }
            else
                return Gender.Unknown;
        }

        private string FormatDate(DateTime? aDateValue)
        {
            if (aDateValue.HasValue)
            {
                return aDateValue.Value.ToString("yyyyMMdd");
            }
            else
                return string.Empty;
        }

        private string FormatDate(DateTime? aDateValue, bool aUseShortDate)
        {
            if (aDateValue.HasValue)
            {
                if (!aUseShortDate)
                    return aDateValue.Value.ToString("yyyyMMdd");
                else
                    return aDateValue.Value.ToString("yyMMdd");
            }
            else
                return String.Empty;
        }

        private string FormatTime(DateTime aDateValue)
        {
            return aDateValue.ToString("HHmm");
        }

        private string FormatAmount(Decimal? aValue)
        {
            var tmpValue = aValue.HasValue ? aValue.Value : 0;
            return tmpValue.ToString("N2").Replace(",", string.Empty);
        }

        private string FormatDecimal(Decimal? aValue)
        {
            var tmpValue = aValue.HasValue ? aValue.Value : 0;
            if ((tmpValue % 1) == 0)
                return tmpValue.ToString("N0").Replace(",", string.Empty);
            else
                return tmpValue.ToString("N2").Replace(",", string.Empty);
        }

        private string FormatDecimal(int? aValue)
        {
            var tmpValue = aValue.HasValue ? aValue.Value : 0;
            return tmpValue.ToString("N0").Replace(",", string.Empty);
        }

        private string FormatNullOrEmptyString(string aValue, string aNullOrEmptyReplacementValue)
        {
            if (!string.IsNullOrWhiteSpace(aValue))
                return aValue;
            else
                return aNullOrEmptyReplacementValue;
        }

        private string GetDisciplineModifier(string aDiscipline)
        {
            switch (aDiscipline.ToUpper())
            {
                case "OT":
                    return "GO";
                case "PT":
                    return "GP";
                case "ST":
                    return "GN";
                default:
                    return "";
            }
        }

        private string FormatInterchangeStr(string aString)
        {
            return StripInvalidChars(aString, BadChars.Interchange_String).Trim();
        }

        private string FormatInterchangeAccountStr(string aString)
        {
            return StripInvalidChars(aString, BadChars.Interchange_Account_String).Trim();
        }

        private string FormatPhone(string aString)
        {
            return StripInvalidChars(aString, BadChars.Phone).Trim();
        }

        private string FormatZip(string aString)
        {
            return StripInvalidChars(aString, BadChars.Zip).Trim();
        }

        private string FormatAccountNo(string aString)
        {
            return StripInvalidChars(aString, BadChars.AccountNo).Trim();
        }

        private string FormatIdentifier(string aString)
        {
            return StripInvalidChars(aString, BadChars.Identifier).Trim();
        }

        private string FormatDx(string aString)
        {
            return StripInvalidChars(aString, BadChars.Diagnosis).Trim();
        }

        private string FormatStr(string aString)
        {
            return StripInvalidChars(aString, BadChars.Str).Trim();
        }
        private string FormatName(string aString)
        {
            return StripInvalidChars(aString, BadChars.Name).Trim();
        }
        private string FormatEmail(string aString)
        {
            return StripInvalidChars(aString, BadChars.Email).Trim();
        }
        private string FormatCity(string aString)
        {
            return StripInvalidChars(aString, BadChars.City).Trim();
        }
        private string StripInvalidChars(string aString, char[] aInvalidChars)
        {
            if (aString != null)
                return new string(aString.Where(c => !aInvalidChars.Contains(c)).ToArray());
            else
                return string.Empty;
        }
        private string ReplaceInvalidChars(string aString, char[] aInvalidChars, char aReplacementChar)
        {
            if (aString != null)
                return new string(aString.Select(c => aInvalidChars.Contains(c) ? aReplacementChar : c).ToArray());
            else
                return string.Empty;
        }
        #endregion
    }
}
