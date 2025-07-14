using Library_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library_Business
{
    public class clsLanguage
    {


            public int? LanguageID { get; set; }
            public string LanguageName { get; set; }

            public clsLanguage()
            {
                LanguageID = null;
                LanguageName = null;
            }

            private clsLanguage(int languageID, string languageName)
            {
                LanguageID = languageID;
                LanguageName = languageName;
            }

            // Method to add a new language
            public bool AddNewLanguage()
            {
                if (string.IsNullOrEmpty(LanguageName))
                {
                    // If LanguageName is not set, return false
                    return false;
                }

                // Add the new language using the data access layer and get the inserted LanguageID
                LanguageID = clsLanguagesData.AddNewLanguage(LanguageName);
                return LanguageID != null;
            }
        public static int? AddNewLanguage(string LanguageName)
        {
            if (string.IsNullOrEmpty(LanguageName))
            {
                // If LanguageName is not set, return false
                return null;
            }

            // Add the new language using the data access layer and get the inserted LanguageID
            return clsLanguagesData.AddNewLanguage(LanguageName);
            
        }

        // Method to update an existing language
        public bool UpdateLanguage()
            {
                if (LanguageID == null || string.IsNullOrEmpty(LanguageName))
                {
                    // If LanguageID or LanguageName is not set, return false
                    return false;
                }

                // Update the language using the data access layer
                return clsLanguagesData.UpdateLanguage(LanguageID, LanguageName);
            }

            // Method to delete a language by its ID
            public static bool DeleteLanguage(int languageID)
            {
                // Delete the language using the data access layer
                return clsLanguagesData.Delete(languageID);
            }

            // Method to find a language by its ID
            public static clsLanguage Find(int languageID)
            {
                string languageName = null;

                // Find the language using the data access layer
                bool isFound = clsLanguagesData.Find(languageID, ref languageName);
                if (isFound)
                {
                    return new clsLanguage(languageID, languageName);
                }

                // If not found, return null
                return null;
            }

            // Method to get the LanguageID from a LanguageName
            public static int? IDOfLanguageName(string languageName)
            {
                // Get the LanguageID using the data access layer
                return clsLanguagesData.IDOfLanguageName(languageName);
            }


             public static bool DoesLanguageExist(String Language)
                {
                return IDOfLanguageName(Language) != null;
            
                }

            // Method to save the language (add new or update)
            public bool Save()
            {
                if (LanguageID == null)
                {
                    // If LanguageID is not set, add new language
                    return AddNewLanguage();
                }
                else
                {
                    // Otherwise, update the existing language
                    return UpdateLanguage();
                }
            }
            public static DataTable GetAllLanguages()
            {
                // Use the data access layer to get all languages
                return clsLanguagesData.GetAllLanguages();
            }
        }
    }

