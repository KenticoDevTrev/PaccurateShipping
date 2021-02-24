using CMS.DocumentEngine;
using CMS.Ecommerce;
using CMS.Helpers;
using CMS.UIControls;
using PaccurateShipping.Models;
using System;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

public partial class CMSModules_PaccurateShipping_Pages_ShipAsIsConfiguration : CMSModalPage
{
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

        ddlItemShippableAsIs.Items.AddRange(new ListItem[]
        {
            new ListItem(ResHelper.LocalizeString("{$ Paccurate.IsShippableSeparate.Default $}"), ""),
            new ListItem(ResHelper.LocalizeString("{$ Paccurate.IsShippableSeparate.Can $}"), "true"),
            new ListItem(ResHelper.LocalizeString("{$ Paccurate.IsShippableSeparate.Cannot $}"), "false"),
        });

        // Get the current Sku based on the node:
        int NodeID = ValidationHelper.GetInteger(URLHelper.GetQueryValue(HttpContext.Current.Request.Url.ToString(), "nodeid"), 0);
        int SkuID = DocumentHelper.GetDocuments().WhereEquals("NodeID", NodeID).Columns("NodeSkuID").FirstOrDefault().NodeSKUID;
        SKUInfo Sku = SKUInfoProvider.GetSKUInfo(SkuID);

        if (Sku.SKUCustomData.GetValue(BoxableSKUItem._BoxableSKUItemSettingKey) != null)
        {
            var CurrentBoxSettings = BoxableSKUItem.XmlToObject(ValidationHelper.GetString(Sku.SKUCustomData.GetValue(BoxableSKUItem._BoxableSKUItemSettingKey),""));
            switch (CurrentBoxSettings.IsShippableSeparate)
            {
                case BoxableSKUItem.ShippableSeparateType.UseSettingsDefault:
                    ddlItemShippableAsIs.SelectedValue = "";
                    break;
                case BoxableSKUItem.ShippableSeparateType.Yes:
                    ddlItemShippableAsIs.SelectedValue = "true";
                    break;
                case BoxableSKUItem.ShippableSeparateType.No:
                    ddlItemShippableAsIs.SelectedValue = "false";
                    break;
            }
            // Set values
            tbxPriceIncreaseRate.Text = CurrentBoxSettings.CostPerWeightUnit.ToString();
            tbxRateWeight.Text = CurrentBoxSettings.WeightRates;
            cbxUseSpecifiedRates.Checked = CurrentBoxSettings.UseSpecifiedRates;
            AdjustVisibilities();
        }
    }

    protected override void OnLoad(EventArgs e)
    {
        AdjustVisibilities();
    }

    private void AdjustVisibilities()
    {
        if (ddlItemShippableAsIs.SelectedValue == "true")
        {
            pnlUseSpecificRates.Visible = true;
            pnlRateWeight.Visible = cbxUseSpecifiedRates.Checked;
            pnlPriceIncreaseRate.Visible = !cbxUseSpecifiedRates.Checked;
        }
        else
        {
            pnlUseSpecificRates.Visible = false;
            pnlRateWeight.Visible = false;
            pnlPriceIncreaseRate.Visible = false;
        }
    }


    protected void cbxUseSpecifiedRates_CheckedChanged(object sender, EventArgs e)
    {
        AdjustVisibilities();
    }

    protected void ddlItemShippableAsIs_SelectedIndexChanged(object sender, EventArgs e)
    {
        AdjustVisibilities();
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        // Build the object and save
        BoxableSKUItem NewItem = new BoxableSKUItem()
        {
            WeightRates = tbxRateWeight.Text,
            CostPerWeightUnit = ValidationHelper.GetDecimal(tbxPriceIncreaseRate.Text, 0),
            UseSpecifiedRates = cbxUseSpecifiedRates.Checked
        };
        switch (ddlItemShippableAsIs.SelectedValue)
        {
            case "":
                NewItem.IsShippableSeparate = BoxableSKUItem.ShippableSeparateType.UseSettingsDefault;
                break;
            case "true":
                NewItem.IsShippableSeparate = BoxableSKUItem.ShippableSeparateType.Yes;
                break;
            case "false":
                NewItem.IsShippableSeparate = BoxableSKUItem.ShippableSeparateType.No;
                break;
        }
        int NodeID = ValidationHelper.GetInteger(URLHelper.GetQueryValue(HttpContext.Current.Request.Url.ToString(), "nodeid"), 0);
        int SkuID = DocumentHelper.GetDocuments().WhereEquals("NodeID", NodeID).Columns("NodeSkuID").FirstOrDefault().NodeSKUID;
        SKUInfo Sku = SKUInfoProvider.GetSKUInfo(SkuID);

        Sku.SKUCustomData.SetValue(BoxableSKUItem._BoxableSKUItemSettingKey, NewItem.ToXML());
        SKUInfoProvider.SetSKUInfo(Sku);

    }
}