using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Transactions;
using System.Web.Mvc;
using DataAccessService.Master;
using DataAccessService.Registrasi;
using DataObjects.Entities;
using FitnessCenter.Areas.Office.Models;
using FitnessCenter.Areas.Office.Views.Card.Model;
using FitnessCenter.Controllers;
using FitnessCenter.Properties;
using MessagingToolkit.QRCode.Codec;
using Services.Class;
using Services.Extensions;
using Services.Helpers;
using ViewModel.Master;

namespace FitnessCenter.Areas.Office.Controllers
{
    public class CardController : FitController
    {
        // GET: Office/Card
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult AddtoPrint(string memberType, int rowCount)
        {
            var dMember = new DataServiceMembership();
            List<trMembership> members = null;
            var cards = new List<EditorMemberCard>();
            var dStatusMember = new DataServiceStatusMember();

            var statusMember = dStatusMember.GetStatusId(EnumStatusMember.Membership);
            var statusCalon = dStatusMember.GetStatusId(EnumStatusMember.CalonMember);

            //            var member = dMember.LoadAllData().OrderByDescending(m => m.trMembershipID).First(p => p.tMember.PersonID == personId &&
            //                        (p.StatusMID == statusMember || p.StatusMID == statusCalon));

            members = (memberType.ToUpper() != "NEW"
                    ? dMember.LoadAllData().Where(p => p.tMember.tMemberType.MemberType.Contains(memberType) && (p.StatusMID == statusMember || p.StatusMID == statusCalon))
                    : dMember.LoadAllData().Where(m => m.CardStatus.Equals(2) && (m.StatusMID == statusMember || m.StatusMID == statusCalon)))
                .ToList();

            rowCount = rowCount == 0 ? 1 : rowCount;

            cards.AddRange(members.Select(tMember => new EditorMemberCard()
            {
                Seq = rowCount++,
                PersonId = tMember.tMember.tPerson.PersonID,
                Alamat = tMember.tMember.tPerson.PAlamat,
                Gender = tMember.tMember.tPerson.PGender,
                Kota = tMember.tMember.tPerson.PKota,
                MemberId = tMember.MemberID,
                MemberName = tMember.tMember.tPerson.PNama,
                TglLahir = (tMember.tMember.tPerson.PTglLahir ?? DateTime.MinValue.AddYears(1899)),
                MemberType = tMember.tMember.tMemberType.MemberType,
                MembershipID = tMember.trMembershipID
            }));

            var listPartialView = cards.Select(card => RazorPartialView.RenderRazorViewToString(ControllerContext, "Editor/EditorMember", card)).ToList();

            return Json(listPartialView, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddDataMember(int personId, int rowCount)
        {
            var dStatusMember = new DataServiceStatusMember();


            var statusMember = dStatusMember.GetStatusId(EnumStatusMember.Membership);
            var statusCalon = dStatusMember.GetStatusId(EnumStatusMember.CalonMember);

            var dMember = new DataServiceMembership();
            var member = dMember.LoadAllData().OrderByDescending(m => m.trMembershipID).First(p => p.tMember.PersonID == personId &&
                        (p.StatusMID == statusMember || p.StatusMID == statusCalon));

            var model = new EditorMemberCard()
            {
                Seq = rowCount + 1,
                PersonId = personId,
                Alamat = member.tMember.tPerson.PAlamat,
                Gender = member.tMember.tPerson.PGender,
                Kota = member.tMember.tPerson.PKota,
                MemberId = member.MemberID,
                MemberName = member.tMember.tPerson.PNama,
                TglLahir = (member.tMember.tPerson.PTglLahir ?? DateTime.MinValue.AddYears(1899)),
                MemberType = member.tMember.tMemberType.MemberType,
                MembershipID = member.trMembershipID
            };

            return PartialView("Editor/EditorMember", model);
        }

        [HttpPost]
        public ActionResult PrintCardMember(Card editor, FormCollection form)
        {
            var dMembership = new DataServiceMembership();
            //            throw new Exception();
            var dStatus = new DataServiceStatusMember();
            var tMembership = new List<ViewModelCardPrint>();
            using (var ts = new TransactionScope())
            {
                foreach (var card in editor.MemberCard)
                {
                    var membership = dMembership.GetobjByID(card.MembershipID);
                    if (membership != null)
                    {
                        membership.CardStatus = 3;
                        dMembership.Update(membership);
                    }
                    else
                    {
                        membership = new trMembership()
                        {
                            MemberID = card.MemberId,
                            PersonBOIDADM = User.Person.PersonID,
                            Subtotal = 5000,
                            Total = 5000,
                            Note = "",
                            LocationID = User.ActiveLocation,
                            StatusMID = dStatus.GetStatusActionId(EnumStatusAction.Card.ToString("F")),
                            CardStatus = 3
                        };
                        dMembership.Insert(membership);
                    }


                    var print = new ViewModelCardPrint()
                    {
                        MemberId = membership.tMember.MemberNO,
                        MemberName = membership.tMember.tPerson.PNama,
                        MemberType = membership.tMember.tMemberType.MemberType
                    };

                    tMembership.Add(print);

                }

                //Export to CSV file
                var folderName = DateTime.Now.ToString("HH-mm-ss");
                var fileName = "Member" + folderName + ".csv";
                string folderPath = $@"C:\FitnessExcel\Card\{folderName}";

                tMembership.ToCSV(path: folderPath, fileName: fileName);


                foreach (var member in tMembership)
                {
                    QRCodeEncoder encoder = new QRCodeEncoder
                    {
                        QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M,
                        QRCodeScale = 10
                    };

                    // 30%

                    Bitmap img = encoder.Encode(@"https:\\secure.flashfitnessindonesia.com\Member\" + member.MemberId);
                    Image logoFullScale = Resources.Logo;
                    Image logo = new Bitmap(logoFullScale, new Size(100, 75));

                    int left = (331 / 2) - (logo.Width / 2);
                    int top = (331 / 2) - (logo.Height / 2);

                    Graphics g = Graphics.FromImage(img);
                    g.DrawImage(logo, new Point(left, top));
                    img.Save(folderPath + "\\" + member.MemberId + ".jpg", ImageFormat.Jpeg);
                }
                //                throw new Exception();
                ts.Complete();
            }

            return RedirectToAction("Index");
        }
    }
}