using Library_DataAccess;
using System;
using System.Data;

namespace Library_Business
{
    public class clsBookGenres
    {
        public int? GenreID { get; set; }
        public string GenreName { get; set; }

        public clsBookGenres()
        {
            GenreID = null;
            GenreName = null;
        }

        private clsBookGenres(int genreID, string genreName)
        {
            GenreID = genreID;
            GenreName = genreName;
        }

        // Method to add a new genre
        public bool AddNewGenre()
        {
            if (string.IsNullOrEmpty(GenreName))
            {
                // If GenreName is not set, return false
                return false;
            }

            // Add the new genre using the data access layer and get the inserted GenreID
            GenreID = clsBookGenresData.AddNewGenre(GenreName);
            return GenreID != null;
        }
        public static int? AddNewGenre(string GenreName)
        {
            if (string.IsNullOrEmpty(GenreName))
            {
                // If GenreName is not set, return false
                return null;
            }

            // Add the new genre using the data access layer and get the inserted GenreID
            return clsBookGenresData.AddNewGenre(GenreName);
            
        }
        // Method to update an existing genre
        public bool UpdateGenre()
        {
            if (GenreID == null || string.IsNullOrEmpty(GenreName))
            {
                // If GenreID or GenreName is not set, return false
                return false;
            }

            // Update the genre using the data access layer
            return clsBookGenresData.UpdateGenre(GenreID, GenreName);
        }

        // Method to delete a genre by its ID
        public static bool DeleteGenre(int genreID)
        {
            // Delete the genre using the data access layer
            return clsBookGenresData.Delete(genreID);
        }

        // Method to find a genre by its ID
        public static clsBookGenres Find(int genreID)
        {
            string genreName = null;

            // Find the genre using the data access layer
            bool isFound = clsBookGenresData.Find(genreID, ref genreName);
            if (isFound)
            {
                return new clsBookGenres(genreID, genreName);
            }

            // If not found, return null
            return null;
        }

        // Method to get the GenreID from a GenreName
        public static int? IDOfGenreName(string genreName)
        {
            // Get the GenreID using the data access layer
            return clsBookGenresData.IDOfGenreName(genreName);
        }


        public static bool DoesGenreExists(string Genre)
        {
            return IDOfGenreName(Genre) != null;
        }

        // Method to save the genre (add new or update)
        public bool Save()
        {
            if (GenreID == null)
            {
                // If GenreID is not set, add new genre
                return AddNewGenre();
            }
            else
            {
                // Otherwise, update the existing genre
                return UpdateGenre();
            }
        }
        public static DataTable GetAllGenres()
        {
            // Use the data access layer to get all genres
            return clsBookGenresData.GetAllGenres();
        }
    }
}
