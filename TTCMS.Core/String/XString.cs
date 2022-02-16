using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

public static class XString
{
    /// <summary>
    /// Mã hóa sang chuỗi dạng base 64
    /// </summary>
    /// <param name="s">Chuỗi cần mã hóa</param>
    /// <returns>Chuỗi đã mã hóa</returns>
    public static String ToBase64(this String s)
    {
        var bytes = Encoding.UTF8.GetBytes(s);
        return Convert.ToBase64String(bytes);
    }

    /// <summary>
    /// Giải mã chuỗi mã hóa base 64
    /// </summary>
    /// <param name="s">Chuỗi đã mã hóa base 64</param>
    /// <returns>Chuỗi đã được giải mã</returns>
    public static String FromBase64(this String s)
    {
        var bytes = Convert.FromBase64String(s);
        return Encoding.UTF8.GetString(bytes);
    }

    /// <summary>
    /// Mã hóa MD5
    /// </summary>
    /// <param name="s">Chuỗi cần mã hóa</param>
    /// <returns>Chuỗi base 64 chứa MD5</returns>
    public static String ToMD5(this String s)
    {
        var bytes = Encoding.UTF8.GetBytes(s);
        var hash = MD5.Create().ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }

    /// <summary>
    /// Chuyển đổi sang tiếng Việt không dấu
    /// </summary>
    /// <param name="s">Chuỗi cần chuyển đổi</param>
    /// <returns>Chuỗi tiếng Việt không dấu</returns>
    public static String ToAscii(this String s)
    {
        String[][] symbols = { 
                                 new String[] { "[áàảãạăắằẳẵặâấầẩẫậ]", "a" }, 
                                 new String[] { "[đ]", "d" },
                                 new String[] { "[éèẻẽẹêếềểễệ]", "e" },
                                 new String[] { "[íìỉĩị]", "i" },
                                 new String[] { "[óòỏõọôốồổỗộơớờởỡợ]", "o" },
                                 new String[] { "[úùủũụưứừửữự]", "u" },
                                 new String[] { "[ýỳỷỹỵ]", "y" },
                                 new String[] { "[/\\s//'\"]", "-" },
                                 new String[] { "[?&]", "" },
                                 new String[] { "[áàảãạăắằẳẵặâấầẩẫậ]", "a" }, 
                                 new String[] { "[đ]", "d" },
                                 new String[] { "[éèẻẽẹêếềểễệ]", "e" },
                                 new String[] { "[íìỉĩị]", "i" },
                                 new String[] { "[óòỏõọôốồổỗộơớờởỡợ]", "o" },
                                 new String[] { "[úùủũụưứừửữự]", "u" },
                                 new String[] { "[ýỳỷỹỵ]", "y" },
                                 new String[] { "[ÁÀẢÃẠĂẮẰẲẴẶÂẤẦẨẪẬ]", "A" },
                                 new String[] { "[Đ]", "D" },
                                 new String[] { "[ÉÈẺẼẸÊẾỀỂỄỆ]", "E" },
                                 new String[] { "[ÍÌỈĨỊ]", "I" },
                                 new String[] { "[ÓÒỎÕỌÔỐỒỔỖỘƠỚỜỞỠỢ]", "O" },
                                 new String[] { "[ÚÙỦŨỤƯỨỪỬỮỰ]", "U" },
                                  new String[] { "[:,+’‘@”“]", "" },
                                 new String[] { "[ÝỲỶỸỴ]", "Y" }
                             };
        foreach (var ss in symbols)
        {
            s = Regex.Replace(s, ss[0], ss[1]);
        }
        return s;
    }
    public static string ReplaceFirst(string text, string search)
    {
        int pos = text.IndexOf(" "+search+" ");
        if (pos < 0)
        {
            return text;
        }
        return text.Substring(0, pos) + string.Format(" <a href=\"/Tag/{0}.html\" title=\"{0}\" target=\"_blank\">{0}</a> ", search) + text.Substring(pos + search.Length);
    }
    public static string CutString(string Fullstring, int length)
    {
        string tam = "";
        int vitri = 0;
        if (Fullstring != null)
        {
            if (Fullstring.Length > length)
            {
                tam = string.Format("{0}", Fullstring.Substring(0, length));
                vitri = tam.LastIndexOf(" ");
                return string.Format("{0}...", tam.Substring(0, vitri));
            }
        }
        
        return Fullstring;
    }
    public static string CutStringVideo(string Fullstring, int length)
    {
        string tam = "";
        int vitri = 0;
        if (Fullstring != null)
        {
            if (Fullstring.Length > length)
            {
                tam = string.Format("{0}", Fullstring.Substring(0, length));
                vitri = tam.LastIndexOf(" ");
                return string.Format("{0}", tam.Substring(0, vitri));
            }
        }

        return Fullstring;
    }
    public static string[] ConvertArray(string TextString)
    {
        string[] StrText = TextString.Split(new[] { ',' }).Select(n => Convert.ToString(n)).ToArray();
        return StrText;
    }
    //
    //Bỏ dấu
    public static String BoDau(this String s)
    {
        String[][] symbols = { 
                                 new String[] { "[áàảãạăắằẳẵặâấầẩẫậ]", "a" }, 
                                 new String[] { "[đ]", "d" },
                                 new String[] { "[éèẻẽẹêếềểễệ]", "e" },
                                 new String[] { "[íìỉĩị]", "i" },
                                 new String[] { "[óòỏõọôốồổỗộơớờởỡợ]", "o" },
                                 new String[] { "[úùủũụưứừửữự]", "u" },
                                 new String[] { "[ýỳỷỹỵ]", "y" },
                                 new String[] { "[ÁÀẢÃẠĂẮẰẲẴẶÂẤẦẨẪẬ]", "A" },
                                 new String[] { "[Đ]", "D" },
                                 new String[] { "[ÉÈẺẼẸÊẾỀỂỄỆ]", "E" },
                                 new String[] { "[ÍÌỈĨỊ]", "I" },
                                 new String[] { "[ÓÒỎÕỌÔỐỒỔỖỘƠỚỜỞỠỢ]", "O" },
                                 new String[] { "[ÚÙỦŨỤƯỨỪỬỮỰ]", "U" },
                                 new String[] { "[ÝỲỶỸỴ]", "Y" }
                             };
        foreach (var ss in symbols)
        {
            s = Regex.Replace(s, ss[0], ss[1]);
        }
        return s;
    }
    public static string SetThuInTuan(DateTime ngay)
    {
        string thuoftuan = "";
        int i = (int)ngay.DayOfWeek;
        switch (i)
        {
            case 0:
                thuoftuan = "Chủ nhật";
                break;
            case 1:
                thuoftuan = "Thứ hai";
                break;
            case 2:
                thuoftuan = "Thứ ba";
                break;
            case 3:
                thuoftuan = "Thứ tư";
                break;
            case 4:
                thuoftuan = "Thứ năm";
                break;
            case 5:
                thuoftuan = "Thứ sáu";
                break;
            case 6:
                thuoftuan = "Thứ 7";
                break;
        }
        return thuoftuan;
    }
    public static decimal ToNumber(this decimal s)
    {
        string k = s.ToString();
        String[][] symbols = { 
                                 new String[] { "[,.đ]", "" }, 
                             };
        foreach (var ss in symbols)
        {
            k = Regex.Replace(k, ss[0], ss[1]);
        }
        return Convert.ToDecimal(k);
    }
    public static string ToDescription(this System.Enum value)
    {
        FieldInfo fi = value.GetType().GetField(value.ToString());
        var attribute = fi.GetCustomAttributes(typeof(DescriptionAttribute), true).FirstOrDefault();
        var title = attribute == null ? value.ToString() : ((DescriptionAttribute)attribute).Description;
        return title;
    }
    public static ICollection<SelectListItem> ToSelectListItems(int maxvalue, string selectedId = "")
    {
        var listnew = new List<SelectListItem>();
        for (int i = 1; i <= maxvalue; i++)
        {
            var listItem = new SelectListItem()
            {
                Text = i.ToString(),
                Value = i.ToString(),
                Selected = selectedId == i.ToString()
            };
            listnew.Add(listItem);
        }
        return listnew;
    }
    public static string DateCalculation(DateTime CreatedDate)
    {
        string notificationdate;
        string creDate = CreatedDate.ToShortDateString();
        string dateNow = DateTime.Now.ToShortDateString();
        TimeSpan duration = DateTime.Now - CreatedDate;
        if (creDate == dateNow)
        {

            if (duration.Hours >= 1)
            {
                if (duration.Hours == 1)
                {
                    return notificationdate = duration.Hours.ToString() + " " + "giờ trước";

                }
                else
                {
                    return notificationdate = duration.Hours.ToString() + " " + "giờ trước";
                }
            }
            else if (duration.Minutes >= 1)
            {
                if (duration.Minutes == 1)
                {
                    return notificationdate = duration.Minutes.ToString() + " " + "phút trước";
                }
                else
                {
                    return notificationdate = duration.Minutes.ToString() + " " + "phút trước";
                }
            }
            else
            {
                return notificationdate = "Vài giây trước ";
            }
        }
        else
        {
            if (duration.Days == 0 && duration.Hours < 24)
            {
                return notificationdate = duration.Hours.ToString() + " " + "giờ trước";
            }
            else
            {
                if (CreatedDate.Date ==  DateTime.Today.AddDays(-1).Date)
                {
                    return notificationdate = "Ngày hôm qua";
                }
                else
                {
                    return notificationdate = string.Format("{0} ngày {1}",string.Format("{0:HH:mm}",CreatedDate),string.Format("{0:dd/MM/yyyy}",CreatedDate));
                }
            }
        }
        //else if (CreatedDate.Date ==  DateTime.Today.AddDays(-1).Date)
        //{
        //    return notificationdate = "Yesterday";
        //}
        //else
        //{
        //    return notificationdate = CreatedDate.ToShortDateString();
        //}
    }
}