using DbModel.DomainClasses.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Student_Information_System.Extentions.Helpers
{
    public class DropDown
    {
        public static SelectList OrderList(Order order)
        {
            var selectListOrder = new List<SelectListItem>
            {
                new SelectListItem {Text = "Asscending", Value = "Asscending"},
                new SelectListItem {Text = "Descending", Value = "Descending"}
            };

            return new SelectList(selectListOrder, "Value", "Text", order);
        }

        public static SelectList CountList(int count)
        {
            var selectListCount = new List<SelectListItem>
            {
                new SelectListItem {Text = "10", Value = "10"},
                new SelectListItem {Text = "30", Value = "30"},
                new SelectListItem {Text = "50", Value = "50"}
            };

            return new SelectList(selectListCount, "Value", "Text", count);
        }

        public static SelectList GetUserSearchByList(UserSearchBy userSearchBy)
        {
            var selectedUserSearchBy = new List<SelectListItem>
            {
                new SelectListItem {Text = "UserName", Value = "UserName"},
                //new SelectListItem {Text = "PhoneNumber", Value = "PhoneNumber"},
                //new SelectListItem {Text = "Ip", Value = "Ip"},
                new SelectListItem {Text = "RoleDescription", Value = "RoleDescription"}
            };

            return new SelectList(selectedUserSearchBy, "Value", "Text", userSearchBy);
        }

        public static SelectList GetUserOrderByList(UserOrderBy usersOrderBy)
        {
            var selectedOrder = new List<SelectListItem>
            {
                new SelectListItem {Text = "UserName", Value = "UserName"},
                new SelectListItem {Text = "RegisterDate", Value = "RegisterDate"},
                //new SelectListItem {Text = "OrderCount", Value = "OrderCount"}
            };
            return new SelectList(selectedOrder, "Value", "Text", usersOrderBy);
        }
        public static SelectList GetCountList(int selected)
        {
            var selectedCount = new List<SelectListItem>
            {
                new SelectListItem {Text = "10", Value = "10"},
                new SelectListItem {Text = "30", Value = "30"},
                new SelectListItem {Text = "50", Value = "50"}
            };
            return new SelectList(selectedCount, "Value", "Text", selected);
        }
        public static SelectList GetOrderList(Order order)
        {
            var selectedUserOrderBy = new List<SelectListItem>
            {
                new SelectListItem {Text = "Descendign", Value = "Descendign"},
                new SelectListItem {Text = "Asscending", Value = "Asscending"}
            };
            return new SelectList(selectedUserOrderBy, "Value", "Text", order);
        }
        //public static SelectList GetUserOperationList(UserOperations userOperations)
        //{
        //    var selectedOperation = new List<SelectListItem>
        //    {
        //        new SelectListItem {Text = "کارهای دسته جمعی", Value = "AllWorks"},
        //        new SelectListItem {Text = "حذف", Value = "DeleteSelectedUses"},
        //        new SelectListItem {Text = "قفل کردن", Value = "BanedSelectedUsers"},
        //        new SelectListItem {Text = "باز کردن قفل", Value = "UnBanedSelectedUsers"}
        //    };

        //    return new SelectList(selectedOperation, "Value", "Text", userOperations);
        //}


        //public static SelectList GetProductOrderByList(ArticleOrderBy articleOrderBy)
        //{

        //    var list = new List<SelectListItem>
        //    {
        //        new SelectListItem {Text = "نام", Value = "Name"},
        //        new SelectListItem {Text = "تعداد در انبار", Value = "StockCount"},
        //        new SelectListItem {Text = "تعداد فروش", Value = "SellCount"},
        //        new SelectListItem {Text = "تعداد مشاهده", Value = "ViewCount"},
        //        new SelectListItem {Text = "تعداد رزرو شده در کارت", Value = "ReserveCount"},
        //        new SelectListItem {Text = "قیمت", Value = "Price"},
        //        new SelectListItem {Text = "درصد تخفیف", Value = "DiscountPercent"},
        //        new SelectListItem {Text = "مینیمم مقدار هشدار", Value = "NotificationStockMinimun"}

        //    };

        //    return new SelectList(list, "Value", "Text", articleOrderBy);
        //}


        public static SelectList GetSearchPageCount(int seleted)
        {

            var list = new List<SelectListItem>
            {
                 new SelectListItem {Text = "12", Value = "12"},
                new SelectListItem {Text = "24", Value = "24"},
                new SelectListItem {Text = "36", Value = "36"},
                new SelectListItem {Text = "48", Value = "48"}
            };

            return new SelectList(list, "Value", "Text", seleted);
        }

        //public static SelectList GetSearchFilters(PSFilter seleted)
        //{

        //    var list = new List<SelectListItem>
        //    {
        //        new SelectListItem {Text = "انتخاب کنید", Value = "All"},
        //         new SelectListItem {Text = "جدیدترین ها", Value = "New"},
        //        new SelectListItem {Text = "پرفروش ترین ها", Value = "MoreSell"},
        //        new SelectListItem {Text = "محبوب ترین ها", Value = "Beloved"},
        //        new SelectListItem {Text = "پربازدید ترین ها", Value = "MoreView"},
        //        new SelectListItem {Text = "تخفیف دار ها", Value = "HasDiscount"},
        //        new SelectListItem {Text = "محصولات موجود", Value = "IsInStock"},
        //        new SelectListItem {Text = "محصولات با ارسال رایگان", Value = "FreeSend"}
        //    };

        //    return new SelectList(list, "Value", "Text", seleted);
        //}
        //public static SelectList GetCommentOrderByList(CommentOrderBy commentOrderBy)
        //{
        //    var list = new List<SelectListItem>
        //    {
        //        new SelectListItem {Text = "تاریخ ثبت", Value = "AddDate"},
        //        new SelectListItem {Text = "وضعیت بررسی", Value = "IsApproved"},
        //        new SelectListItem {Text = "تعداد لایک", Value = "LikeCount"},
        //        new SelectListItem {Text = "نام کاربری", Value = "UserName"},
        //        new SelectListItem {Text = "نویسنده", Value = "Author"},
        //        new SelectListItem {Text = "Ip", Value = "Ip"}

        //    };

        //    return new SelectList(list, "Value", "Text", commentOrderBy);
        //}
        //public static SelectList GetTagsOrderByList(TagsOrderBy tagsOrderBy)
        //{
        //    var list = new List<SelectListItem>
        //    {
        //        new SelectListItem {Text = "شماره", Value = "Tag_id"},
        //        new SelectListItem {Text = "نام", Value = "name"}
        //    };
        //    return new SelectList(list, "Value", "Text", tagsOrderBy);
        //}
        //public static SelectList GetAdvertismentOrderByList(AdvertismentOrderBy tagsOrderBy)
        //{
        //    var list = new List<SelectListItem>
        //    {
        //        new SelectListItem {Text = "شماره", Value = "Id"},
        //        new SelectListItem {Text = "عنوان", Value = "Title"}
        //    };
        //    return new SelectList(list, "Value", "Text", tagsOrderBy);
        //}
    }
}