using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;

namespace rasp
{
    public class Lead
    {
        public string name;
        public string lastname;
        public List<String> phones;
        public string email;
        public string adress;
        public string birthdate;

        public Lead(string name, string lastname, List<string> phones, string email, string adress, string birthdate)
        {
            Regex regexPhone = new Regex(@"^\d*$");
            bool resPhone = true;
            foreach (string phone in phones)
            {
                resPhone = regexPhone.IsMatch(phone);
                if (!resPhone)
                    break;
            }

            Regex regexDate = new Regex(@"^([0-9]{4})[\-]([0]?[1-9]|[1][0-2])[\-](0?[1-9]|[12][0-9]|3[01])$");
            bool resDate = regexDate.IsMatch(birthdate);

            string error = " ";
            if (resPhone && resDate)
            {

                if (name != "" && lastname != "" && email != "" && adress != "")
                {
                    this.name = name;
                    this.lastname = lastname;
                    this.phones = phones;
                    this.email = email;
                    this.adress = adress;
                }
                else
                {
                    throw "Введите все поля контакта!";
                }
            }
            else if (!resPhone)
            {
                throw "В поле телефон должны быть только цифры!";
            }
            else
            {
                throw "Неправильный формат ввода дня рождения";
            }

        }
    }

    public class TOOL
    {
        public List<Lead> DATA = new List<Lead>();
        const string leadsFile = "data.txt";
        public int index = 0;

        public List<Lead> readFile()
        {
            DATA.Clear();
            StreamReader streamReader = new StreamReader(leadsFile);
            string str = "";
            while (!streamReader.EndOfStream)
            {
                str = streamReader.ReadLine();

                List<String> list = new List<String>(str.Split(' '));
                List<String> phones = new List<String>(list[2].Replace('[', ' ').Replace(']', ' ').Split(','));
                Lead lead = new Lead(list[0], list[1], phones, list[3], list[4], list[5]);
                DATA.Add(lead);
            }
            streamReader.Close();
            return DATA;
        }

        public List<Lead> findLeads(string name, string lastname, string phone)
        {
            List<Lead> Data = new List<Lead>();
            foreach (Lead l in DATA)
            {
                string patternName = @"" + name + "";
                string patternLastName = @"" + lastname + "";
                string patternPhone = @"" + phone + "";

                Regex regex1 = new Regex(patternName);
                Match match1 = regex1.Match(l.name);

                Regex regex2 = new Regex(patternLastName);
                Match match2 = regex2.Match(l.lastname);

                Regex regex3 = new Regex(patternPhone);
                bool match3Success = false;
                foreach (string phone1 in l.phones)
                {
                    if (regex3.Match(phone1).Success)
                    {
                        match3Success = true;
                        break;
                    }
                }

                if (match1.Success && match2.Success && match3Success)
                    Data.Add(l);

            }
            return Data;
        }

        public void appendLead(string name, string lastname, List<String> phones, string email, string adress, string birthdate)
        {
            Lead newLead = new Lead(name, lastname, phones, email, adress, birthdate);
            bool flag = true;
            foreach (Lead l in DATA)
            {
                if (l.name == name && l.lastname == lastname && new HashSet<string>(l.phones) == new HashSet<string>(phones) && l.email == email && l.adress == adress && l.birthdate == birthdate)
                {
                    flag = false;
                    error = "Извините, но контакт с такими данными уже существует!";
                    break;
                }
            }

            if (flag)
            {
                StreamWriter sw = new StreamWriter(leadsFile, true);
                sw.WriteLine(name + " " + lastname + " " + phones + " " + email + " " + adress + " " + birthdate);
                sw.Close();
            }
            else
                return error;
        }

        public void rewriteLeads()
        {
            StreamWriter sr = new StreamWriter(leadsFile, false);
            foreach (Lead l in DATA)
            {
                sr.WriteLine(l.name + " " + l.lastname + " " + l.phones.ToString() + " " + l.email + " " + l.adress + " " + l.birthdate);
            }
            sr.Close();
        }

        public string editLead(string name, string lastname, List<string> phones, string email, string adress, string birthdate)
        {
            if (name != "" && lastname != "" && email != "" && adress != "" && birthdate != "")
            {
                DATA[index].name = name;
                DATA[index].lastname = lastname;
                DATA[index].phones = phones;
                DATA[index].email = email;
                DATA[index].adress = adress;
                DATA[index].birthdate = birthdate;

                return "OK";
            }
            else
            {
                return "Введите все поля контакта!";
            }
        }
    }
}