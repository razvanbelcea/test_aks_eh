namespace eathappy.order.domain.Common
{
    public static class ErrorConstants
    {
        public const string InvalidActionOnConfirmedOrder = "Cannot perform this action on confirmed order.";
        public const string InvalidArticleQuantity = "Quantity provided is not valid.";

        public static class ErrorCodes
        {
            public const string InvalidHubName = "HUBNAME_IS_MISSING";
            public const string InvalidOrderState = "INVALID_ORDER_STATE";
            public const string InvalidOrderDate = "INVALID_ORDER_DATE";
            public const string InvalidDeliveryDate = "INVALID_ORDER_DELIVERY_DATE";
            public const string InvalidArticleNumber = "ARTICLE_SKU_IS_INVALID";
            public const string NegativeQuantityNotAllowed = "NEGATIVE_QUANTITY_NOT_ALLOWED";
            public const string DeliveryDateBeforeOrderDate = "DELIVERYDATE_BEFORE_ORDERDATE";
        }

        public static class ErrorDescription
        {
            public const string InvalidHubName = "Cannot add order without specifing Hub Name.";
            public const string InvalidOrderState = "Cannot add order with state Confirmed.";
            public const string InvalidOrderDate = "Order date is not valid.";
            public const string InvalidDeliveryOrderDate = "Order delivery date is invalid.";
            public const string InvalidArticleNumber = "Article number is invalid.";
            public const string NegativeQuantityNotAllowed = "Negative quantity is not allowed for orders.";
            public const string DeliveryDateBeforeOrderDate = "Delivery date cannot be before order date.";

        }
    }
}
