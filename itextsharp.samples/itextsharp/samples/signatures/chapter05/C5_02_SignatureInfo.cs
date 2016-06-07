/*

This file is part of the iText (R) project.
Copyright (c) 1998-2016 iText Group NV

*/
/*
* This class is part of the white paper entitled
* "Digital Signatures for PDF documents"
* written by Bruno Lowagie
*
* For more info, go to: http://itextpdf.com/learn
*/
using System;
using System.Collections.Generic;
using System.IO;
using Org.BouncyCastle.Tsp;
using Org.BouncyCastle.X509;
using iTextSharp.Forms;
using iTextSharp.Kernel.Geom;
using iTextSharp.Kernel.Pdf;
using iTextSharp.Kernel.Pdf.Annot;
using iTextSharp.Signatures;

namespace iTextSharp.Samples.Signatures.Chapter05
{
	public class C5_02_SignatureInfo : C5_01_SignatureIntegrity
	{
        public static readonly string EXAMPLE1 = NUnit.Framework.TestContext.CurrentContext.TestDirectory + "/../../resources/pdfs/step_4_signed_by_alice_bob_carol_and_dave.pdf";

        public static readonly string EXAMPLE4 = NUnit.Framework.TestContext.CurrentContext.TestDirectory + "/../../resources/pdfs/hello_signed4.pdf";

        public static readonly string EXAMPLE6 = NUnit.Framework.TestContext.CurrentContext.TestDirectory + "/../../resources/pdfs/field_metadata.pdf";

	    public const String expectedOutput = "===== sig1 =====\n" + "\n" + "Field on page 1; llx: 36.0, lly: 728.02, urx: 559.0; ury: 779.02\n"
			 + "Signature covers whole document: false\n" + "Document revision: 1 of 4\n" + 
			"Integrity check OK? true\n" + "Digest algorithm: SHA256\n" + "Encryption algorithm: RSA\n"
			 + "Filter subtype: /adbe.pkcs7.detached\n" + "Name of the signer: Alice Specimen\n"
			 + "Signed on: 2016-02-23\n" + "Location: \n" + "Reason: \n" + "Contact info: \n"
			 + "Signature type: certification\n" + "Filling out fields allowed: true\n" + "Adding annotations allowed: false\n"
			 + "===== sig2 =====\n" + "\n" + "\n" + "\n" + "Field on page 1; llx: 36.0, lly: 629.04, urx: 559.0; ury: 680.04\n"
			 + "Signature covers whole document: false\n" + "Document revision: 2 of 4\n" + 
			"Integrity check OK? true\n" + "Digest algorithm: SHA256\n" + "Encryption algorithm: RSA\n"
			 + "Filter subtype: /adbe.pkcs7.detached\n" + "Name of the signer: Bob Specimen\n"
			 + "Signed on: 2016-02-23\n" + "Location: \n" + "Reason: \n" + "Contact info: \n"
			 + "Signature type: approval\n" + "Filling out fields allowed: true\n" + "Adding annotations allowed: false\n"
			 + "Lock: /Include[sig1 approved_bob sig2 ]\n" + "===== sig3 =====\n" + "\n" + "\n"
			 + "\n" + "\n" + "\n" + "Field on page 1; llx: 36.0, lly: 530.05, urx: 559.0; ury: 581.05\n"
			 + "Signature covers whole document: false\n" + "Document revision: 3 of 4\n" + 
			"Integrity check OK? true\n" + "Digest algorithm: SHA256\n" + "Encryption algorithm: RSA\n"
			 + "Filter subtype: /adbe.pkcs7.detached\n" + "Name of the signer: Carol Specimen\n"
			 + "Signed on: 2016-02-23\n" + "Location: \n" + "Reason: \n" + "Contact info: \n"
			 + "Signature type: approval\n" + "Filling out fields allowed: true\n" + "Adding annotations allowed: false\n"
			 + "Lock: /Include[sig1 approved_bob sig2 ]\n" + "Lock: /Exclude[approved_dave sig4 ]\n"
			 + "===== sig4 =====\n" + "\n" + "\n" + "\n" + "\n" + "\n" + "\n" + "\n" + "Field on page 1; llx: 36.0, lly: 431.07, urx: 559.0; ury: 482.07\n"
			 + "Signature covers whole document: true\n" + "Document revision: 4 of 4\n" + "Integrity check OK? true\n"
			 + "Digest algorithm: SHA256\n" + "Encryption algorithm: RSA\n" + "Filter subtype: /adbe.pkcs7.detached\n"
			 + "Name of the signer: Dave Specimen\n" + "Signed on: 2016-02-23\n" + "Location: \n"
			 + "Reason: \n" + "Contact info: \n" + "Signature type: approval\n" + "Filling out fields allowed: false\n"
			 + "Adding annotations allowed: false\n" + "Lock: /Include[sig1 approved_bob sig2 ]\n"
			 + "Lock: /Exclude[approved_dave sig4 ]\n" + "\n" + "===== sig =====\n" + "\n" +
			 "Field on page 1; llx: 36.0, lly: 648.0, urx: 236.0; ury: 748.0\n" + "Signature covers whole document: true\n"
			 + "Document revision: 1 of 1\n" + "Integrity check OK? true\n" + "Digest algorithm: RIPEMD160\n"
			 + "Encryption algorithm: RSA\n" + "Filter subtype: /ETSI.CAdES.detached\n" + "Name of the signer: Bruno Specimen\n"
			 + "Signed on: 2016-02-23\n" + "Location: Ghent\n" + "Reason: Test 4\n" + "Contact info: \n"
			 + "Signature type: approval\n" + "Filling out fields allowed: true\n" + "Adding annotations allowed: true\n"
			 + "\n" + "===== Signature1 =====\n" + "\n" + "Field on page 1; llx: 46.0674, lly: 472.172, urx: 332.563; ury: 726.831\n"
			 + "Signature covers whole document: true\n" + "Document revision: 1 of 1\n" + "Integrity check OK? true\n"
			 + "Digest algorithm: SHA256\n" + "Encryption algorithm: RSA\n" + "Filter subtype: /adbe.pkcs7.detached\n"
			 + "Name of the signer: Bruno Specimen\n" + "Alternative name of the signer: Bruno L. Specimen\n"
			 + "Signed on: 2016-02-23\n" + "Location: Ghent\n" + "Reason: Test metadata\n" +
			 "Contact info: 555 123 456\n" + "Signature type: approval\n" + "Filling out fields allowed: true\n"
			 + "Adding annotations allowed: true\n" + "\n";

		// public static final String EXAMPLE2 = "results/chapter3/hello_cacert_ocsp_ts.pdf";
		// public static final String EXAMPLE3 = "results/chapter3/hello_token.pdf";
		// public static final String EXAMPLE5 = "results/chapter4/hello_smartcard_Signature.pdf";
		/// <exception cref="Org.BouncyCastle.Security.GeneralSecurityException"/>
		/// <exception cref="System.IO.IOException"/>
		public virtual SignaturePermissions InspectSignature(PdfDocument pdfDoc, SignatureUtil
			 signUtil, PdfAcroForm form, String name, SignaturePermissions perms)
		{
			if (form.GetField(name).GetWidgets() != null && form.GetField(name).GetWidgets().
				Count > 0)
			{
				int pageNum = 0;
				Rectangle pos = form.GetField(name).GetWidgets()[0].GetRectangle().ToRectangle();
				for (int i = 1; i <= pdfDoc.GetNumberOfPages(); i++)
				{
					PdfPage page = pdfDoc.GetPage(i);
					foreach (PdfAnnotation annot in page.GetAnnotations())
					{
						System.Console.Out.WriteLine();
						if (PdfName.Sig.Equals(annot.GetPdfObject().Get(PdfName.FT)) && name.Equals(annot
							.GetPdfObject().Get(PdfName.T).ToString()))
						{
							pageNum = pdfDoc.GetPageNumber(page);
							break;
						}
					}
				}
				if (pos.GetWidth() == 0 || pos.GetHeight() == 0)
				{
					System.Console.Out.WriteLine("Invisible signature");
				}
				else
				{
					System.Console.Out.WriteLine(String.Format("Field on page %s; llx: %s, lly: %s, urx: %s; ury: %s"
						, pageNum, pos.GetLeft(), pos.GetBottom(), pos.GetRight(), pos.GetTop()));
				}
			}
			PdfPKCS7 pkcs7 = base.VerifySignature(signUtil, name);
			System.Console.Out.WriteLine("Digest algorithm: " + pkcs7.GetHashAlgorithm());
			System.Console.Out.WriteLine("Encryption algorithm: " + pkcs7.GetEncryptionAlgorithm
				());
			System.Console.Out.WriteLine("Filter subtype: " + pkcs7.GetFilterSubtype());
			X509Certificate cert = (X509Certificate)pkcs7.GetSigningCertificate();
            System.Console.Out.WriteLine("Name of the signer: " + iTextSharp.Signatures.CertificateInfo.GetSubjectFields
				(cert).GetField("CN"));
			if (pkcs7.GetSignName() != null)
			{
				System.Console.Out.WriteLine("Alternative name of the signer: " + pkcs7.GetSignName
					());
			}
            System.Console.Out.WriteLine("Signed on: " + pkcs7.GetSignDate().ToUniversalTime().ToString("yyyy-MM-dd"));
			if (pkcs7.GetTimeStampDate() != null)
			{
                System.Console.Out.WriteLine("TimeStamp: " + pkcs7.GetTimeStampDate().ToUniversalTime().ToString("yyyy-MM-dd"));
				TimeStampToken ts = pkcs7.GetTimeStampToken();
				System.Console.Out.WriteLine("TimeStamp service: " + ts.TimeStampInfo.Tsa);
				System.Console.Out.WriteLine("Timestamp verified? " + pkcs7.VerifyTimestampImprint
					());
			}
			System.Console.Out.WriteLine("Location: " + pkcs7.GetLocation());
			System.Console.Out.WriteLine("Reason: " + pkcs7.GetReason());
			PdfDictionary sigDict = signUtil.GetSignatureDictionary(name);
			PdfString contact = sigDict.GetAsString(PdfName.ContactInfo);
			if (contact != null)
			{
				System.Console.Out.WriteLine("Contact info: " + contact);
			}
			perms = new SignaturePermissions(sigDict, perms);
			System.Console.Out.WriteLine("Signature type: " + (perms.IsCertification() ? "certification"
				 : "approval"));
			System.Console.Out.WriteLine("Filling out fields allowed: " + perms.IsFillInAllowed
				());
			System.Console.Out.WriteLine("Adding annotations allowed: " + perms.IsAnnotationsAllowed
				());
			foreach (SignaturePermissions.FieldLock Lock in perms.GetFieldLocks())
			{
				System.Console.Out.WriteLine("Lock: " + Lock.ToString());
			}
			return perms;
		}

		/// <exception cref="System.IO.IOException"/>
		/// <exception cref="Org.BouncyCastle.Security.GeneralSecurityException"/>
		public virtual void InspectSignatures(String path)
		{
			// System.out.println(path);
			PdfDocument pdfDoc = new PdfDocument(new PdfReader(path));
			PdfAcroForm form = PdfAcroForm.GetAcroForm(pdfDoc, false);
			SignaturePermissions perms = null;
			SignatureUtil signUtil = new SignatureUtil(pdfDoc);
			IList<String> names = signUtil.GetSignatureNames();
			foreach (String name in names)
			{
				System.Console.Out.WriteLine("===== " + name + " =====");
				perms = InspectSignature(pdfDoc, signUtil, form, name, perms);
			}
			System.Console.Out.WriteLine();
		}

		/// <exception cref="System.IO.IOException"/>
		/// <exception cref="Org.BouncyCastle.Security.GeneralSecurityException"/>
		public static void Main(String[] args)
		{
			C5_02_SignatureInfo app = new C5_02_SignatureInfo();
			app.InspectSignatures(EXAMPLE1);
			// app.inspectSignatures(EXAMPLE2);
			// app.inspectSignatures(EXAMPLE3);
			app.InspectSignatures(EXAMPLE4);
			// app.inspectSignatures(EXAMPLE5);
			app.InspectSignatures(EXAMPLE6);
		}

		/// <exception cref="Org.BouncyCastle.Security.GeneralSecurityException"/>
		/// <exception cref="System.IO.IOException"/>
		/// <exception cref="System.Exception"/>
		/// <exception cref="Javax.Smartcardio.CardException"/>
		[NUnit.Framework.Test]
		public override void RunTest()
		{
			SetupSystemOutput();
			C5_02_SignatureInfo.Main(null);
			String sysOut = GetSystemOutput();
			NUnit.Framework.Assert.AreEqual(expectedOutput, sysOut, "Unexpected output");
		}
	}
}
