﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;

public partial class Ikat : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        #region Session

        if (Session["SessionID"] != null)
        {
            sessionID = Session["SessionID"].ToString();
            divLogIn.Visible = false;
            divIkat.Visible = true;

            if (!IsPostBack)
                SetTranslator();
        }
        else
        {
            tbPassKey.Focus();
            divLogIn.Visible = true;
            divIkat.Visible = false;
        }

        #endregion
    }

    #region Control Events

    protected void btnLogin_Click(object sender, EventArgs e)
    {
        if (tbPassKey.Text == passKey)
        {
            sessionID = Guid.NewGuid().ToString();
            Session["SessionID"] = sessionID;

            divLogIn.Visible = false;
            divIkat.Visible = true;
            SetTranslator();
        }
        else
        {
            divLogIn.Visible = true;
            divIkat.Visible = false;
            tbPassKey.Text = "";
            tbPassKey.Focus();
        }
    }

    protected void btnLanguages_Click(object sender, EventArgs e)
    {
        SetLanguages();
    }

    protected void btnTranslator_Click(object sender, EventArgs e)
    {
        SetTranslator();
    }

    protected void btnLogOff_Click(object sender, EventArgs e)
    {
        Session["SessionID"] = null;
        Response.Redirect("Ikat.aspx");
    }

    protected void btnGoWebsite_Click(object sender, EventArgs e)
    {
        string baseUrl = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/') + "/";
        Response.Redirect(baseUrl);
    }

    protected void rptLanguages_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        XmlNode nodLanguage = (XmlNode)e.Item.DataItem;
        TextBox tbCode = (TextBox)e.Item.FindControl("tbCode");
        TextBox tbDescription = (TextBox)e.Item.FindControl("tbDescription");
        Button btnUpdateLanguage = (Button)e.Item.FindControl("btnUpdateLanguage");
        Button btnRemoveLanguage = (Button)e.Item.FindControl("btnRemoveLanguage");

        btnUpdateLanguage.CommandArgument = nodLanguage.Attributes["id"].Value;
        btnRemoveLanguage.CommandArgument = nodLanguage.Attributes["id"].Value;
        tbCode.Text = nodLanguage.Attributes["code"].Value;
        tbDescription.Text = nodLanguage.Attributes["description"].Value;
    }

    protected void btnUpdateLanguage_Click(object sender, EventArgs e)
    {
        Button btnUpdateLanguage = (Button)sender;
        RepeaterItem ri = (RepeaterItem)btnUpdateLanguage.Parent;
        TextBox tbDescription = (TextBox)ri.FindControl("tbDescription");
        Mobrol.SaveLanguage(btnUpdateLanguage.CommandArgument, "", tbDescription.Text);
    }

    protected void btnRemoveLanguage_Click(object sender, EventArgs e)
    {
        Button btnRemoveLanguage = (Button)sender;
        Mobrol.RemoveLanguage(btnRemoveLanguage.CommandArgument);
        Mobrol.GetLanguages(rptLanguages);
    }

    protected void btnRefreshTerms_Click(object sender, EventArgs e)
    {
        Mobrol.GetTerms(rptTerms);
    }

    protected void cbForceMultiLine_CheckedChanged(object sender, EventArgs e)
    {
        btnSaveTerm_Click(this, new EventArgs());
        SetTranslator();
    }

    protected void btnAddLanguage_Click(object sender, EventArgs e)
    {
        if (tbNewCode.Text.Length > 0 && tbNewDescription.Text.Length > 0)
        {
            Mobrol.SaveLanguage("", tbNewCode.Text, tbNewDescription.Text);
            tbNewCode.Text = "";
            tbNewDescription.Text = "";
        }
        else
        {
            if (tbNewCode.Text.Length == 0)
                tbNewCode.Focus();
            else
                tbNewDescription.Focus();
        }

        Mobrol.GetLanguages(rptLanguages);
    }

    protected void rptTerms_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        XmlNode nodTerm = (XmlNode)e.Item.DataItem;
        LinkButton btnTermID = (LinkButton)e.Item.FindControl("btnTermID");
        LinkButton btnDeleteTerm = (LinkButton)e.Item.FindControl("btnDeleteTerm");
        Label lblTermID = (Label)e.Item.FindControl("lblTermID");

        btnTermID.CommandArgument = nodTerm.Attributes["id"].Value;
        btnDeleteTerm.CommandArgument = nodTerm.Attributes["id"].Value;
        lblTermID.Text = nodTerm.Attributes["referer"].Value;

        if (e.Item.ItemIndex == 0 && hfTermID.Value.Length == 0)
        {
            if (!btnCancelTerm.Visible)
            SetTerm(nodTerm.Attributes["id"].Value);
        }
        else
        { 
            if (nodTerm.Attributes["id"].Value == hfTermID.Value)
                SetTerm(nodTerm.Attributes["id"].Value);
        }
    }

    protected void btnTermID_Click(object sender, EventArgs e)
    {
        LinkButton btnTermID = (LinkButton)sender;
        SetTerm(btnTermID.CommandArgument);
        Mobrol.GetTerms(rptTerms);
        btnCancelTerm.Visible = false;
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "setScrollposition", "setScrollposition();", true);
    }

    protected void btnDeleteTerm_Click(object sender, EventArgs e)
    {
        LinkButton btnDeleteTerm = (LinkButton)sender;
        DeleteTerm(btnDeleteTerm.CommandArgument);
    }

    protected void btnAddTerm_Click(object sender, EventArgs e)
    {
        btnCancelTerm.CommandArgument = hfTermID.Value;
        SetTerm("");
        tbReferer.Focus();
        btnCancelTerm.Visible = true;
        Mobrol.GetTerms(rptTerms);
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "setScrollposition", "setScrollposition();", true);
    }

    protected void rptTranslations_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        XmlNode nodLanguage = (XmlNode)e.Item.DataItem;
        HiddenField hfLanguageCode = (HiddenField)e.Item.FindControl("hfLanguageCode");
        Label lblDescription = (Label)e.Item.FindControl("lblDescription");
        TextBox tbTranslation = (TextBox)e.Item.FindControl("tbTranslation");
        HtmlImage imgTranslate = (HtmlImage)e.Item.FindControl("imgTranslate");

        hfLanguageCode.Value = nodLanguage.Attributes["code"].Value;
        lblDescription.Text = nodLanguage.Attributes["description"].Value;
        imgTranslate.Attributes.Add("onclick", "window.open('https://translate.google.com/#en/" + nodLanguage.Attributes["code"].Value.ToLower() + "/" + tbDefaultText.Text.Replace("&#xA;", "").Replace("\n", "") + "', 'GoogleTranslate');");

        XmlNode nodTranslation = Mobrol.GetTranslation(hfTermID.Value, nodLanguage.Attributes["code"].Value);
        tbTranslation.Text = nodTranslation.Attributes["text"].Value;
        tbTranslation.Attributes.Add("ondblclick", "copyClipBoard('" + tbTranslation.ClientID + "');");

        if (cbForceMultiLine.Checked || multiLine)
            tbTranslation.Rows = 7;
        else
            tbTranslation.Rows = 1;
    }

    protected void btnSaveTerm_Click(object sender, EventArgs e)
    {
        string termID = Mobrol.SaveTerm(hfTermID.Value, tbReferer.Text, tbDefaultText.Text);

        foreach (RepeaterItem ri in rptTranslations.Items)
        {
            HiddenField hfLanguageCode = (HiddenField)ri.FindControl("hfLanguageCode");
            TextBox tbTranslation = (TextBox)ri.FindControl("tbTranslation");

            Mobrol.SaveTranslation(termID, hfLanguageCode.Value, tbTranslation.Text);
        }

        hfTermID.Value = termID;
        SetTranslator();
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "setScrollposition", "setScrollposition();", true);
    }

    protected void btnCancelTerm_Click(object sender, EventArgs e)
    {
        hfTermID.Value = btnCancelTerm.CommandArgument;
        SetTranslator();
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "setScrollposition", "setScrollposition();", true);
        btnCancelTerm.Visible = false;
    }

    #endregion

    #region Helper Functions

    private void SetLanguages()
    {
        lblHeader.Text = btnLanguages.Text;
        btnLanguages.Visible = false;
        btnTranslator.Visible = true;
        divLanguages.Visible = true;
        divTerms.Visible = false;
        Mobrol.GetLanguages(rptLanguages);
    }

    private void SetTranslator()
    {
        lblHeader.Text = btnTranslator.Text;
        btnLanguages.Visible = true;
        btnTranslator.Visible = false;
        divLanguages.Visible = false;
        divTerms.Visible = true;

        if (cbForceMultiLine.Checked)
            tbDefaultText.Rows = 7;
        else
            tbDefaultText.Rows = 1;

        Mobrol.GetTerms(rptTerms);
    }

    private void SetTerm(string termID)
    {
        XmlNode nodTerm = Mobrol.GetTerm(termID);

        if (nodTerm != null)
        {
            #region Set Term Values

            hfTermID.Value = nodTerm.Attributes["id"].Value;
            tbReferer.Text = nodTerm.Attributes["referer"].Value;
            tbDefaultText.Text = nodTerm.Attributes["text"].Value;

            #endregion

            #region Set Default TextBox Height

            tbDefaultText.Rows = 1;
            multiLine = (tbDefaultText.Text.Length > maxSingleLength);

            #endregion

            #region Enforce MultiLine

            if (!cbForceMultiLine.Checked)
            {
                if (tbDefaultText.Text.Length > maxSingleLength)
                {
                    multiLine = true;
                    tbDefaultText.Rows = 7;
                }
                else
                {
                    multiLine = false;
                    tbDefaultText.Rows = 1;
                }
            }
            else
            { 
                tbDefaultText.Rows = 7;
            }

            #endregion

            Mobrol.GetLanguages(rptTranslations);
        }
        else
        {
            hfTermID.Value = "";
            tbReferer.Text = "";
            tbDefaultText.Text = "";

            rptTranslations.DataSource = null;
            rptTranslations.DataBind();
        }
    }

    private void DeleteTerm(string termID)
    {
        Mobrol.DeleteTerm(termID);
        SetTranslator();
    }

    #endregion

    #region Privates

    private string sessionID = "";
    private string passKey = "IKAT2018";

    private int maxSingleLength = 100;
    private bool multiLine = false;

    #endregion
}