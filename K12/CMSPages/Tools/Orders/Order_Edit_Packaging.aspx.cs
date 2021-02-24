using System;
using CMS.Base.Web.UI;
using CMS.Core;
using CMS.Ecommerce;
using CMS.Ecommerce.Web.UI;
using CMS.Helpers;
using CMS.UIControls;


[UIElement(ModuleName.ECOMMERCE, "Orders.Packaging")]
public partial class CMSModules_Ecommerce_Pages_Tools_Orders_Order_Edit_Packaging : CMSEcommercePage
{
    #region "Variables"

    private int orderId;
    private OrderInfo order;

    #endregion


    #region "Page events"

    protected void Page_Load(object sender, EventArgs e)
    {
        // Register the dialog script
        ScriptHelper.RegisterClientScriptBlock(this, typeof(string), ScriptHelper.NEWWINDOW_SCRIPT_KEY, ScriptHelper.NewWindowScript);

        if (QueryHelper.GetInteger("orderid", 0) != 0)
        {
            orderId = QueryHelper.GetInteger("orderid", 0);
        }
        order = OrderInfoProvider.GetOrderInfo(orderId);

        if (order == null)
        {
            return;
        }

        // Check order site ID
        CheckEditedObjectSiteID(order.OrderSiteID);

        PackagingContent.Text += order.OrderCustomData.GetValue(PaccurateShipping.PaccurateShippingHelper.PaccurateImageField);
        PackagingContent.Text += order.OrderCustomData.GetValue(PaccurateShipping.PaccurateShippingHelper.PaccurateMessageField);
    }

    #endregion
}
