﻿using System.Data.SQLite;
using System.IO;

namespace MP7_progi.Models
{
    public class DatabaseFunctions
    {
        private static string connectionString = @"Data Source=..\..\MP7.db";

        public static void InitializeDB()
        {
            if(!File.Exists(@"..\..\MP7.db"))
            {
                SQLiteConnection.CreateFile(@"..\..\MP7.db");

                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    //Kreiramo tablice
                    string createKorisnikTableQuery = @"
                        CREATE TABLE IF NOT EXISTS Korisnik(
                        userID INTEGER PRIMARY KEY, 
                        ime TEXT,
                        prezime TEXT,
                        email TEXT UNIQUE,
                        telBroj INTEGER UNIQUE,
                        lozinka TEXT,
                        korIme TEXT UNIQUE,
                        nazSklon TEXT UNIQUE CHECK (ime IS NULL OR prezime IS NULL)
                     );";

                    string createKomunikacijaTableQuery = @"
                        CREATE TABLE IF NOT EXISTS Komunikacija(
                        porID INTEGER PRIMARY KEY,
                        lokPor TEXT,
                        slikaPor TEXT,
                        tekstPor TEXT,
                        oglasID INTEGER,
                        FOREIGN KEY(oglasID) REFERENCES Oglas(oglasID)
                     );";

                    string createLjubimacTableQuery = @"
                        CREATE TABLE IF NOT EXISTS Ljubimac(
                        petID INTEGER,
                        imeLjub TEXT,
                        vrsta TEXT,
                        boja TEXT,
                        starost TEXT,
                        datSatNest DATETIME,
                        lokacija TEXT,
                        tekstniOpis TEXT,
                        oglasID INTEGER,
                        PRIMARY KEY(petID),
                        FOREIGN KEY(oglasID) REFERENCES Oglas(oglasID)
                     );";

                    string createOglasTableQuery = @"
                        CREATE TABLE IF NOT EXISTS Oglas (
                        oglasID INTEGER PRIMARY KEY,
                        katOglas TEXT CHECK (katOglas IN ('u potrazi', 'sretno pronađen', 'nije pronađen', 'pronađen uz nesretne okolnosti'))
                     );";

                    string createpisePorTableQuery = @"
                        CREATE TABLE IF NOT EXISTS pisePor(
                        userID INTEGER,
                        porID INTEGER,
                        PRIMARY KEY(userID, porID),
                        FOREIGN KEY(userID) REFERENCES Korisnik(userID),
                        FOREIGN KEY (porID) REFERENCES Komunikacija(porID)
                     );";

                    string createslikeOglasTableQuery = @"
                        CREATE TABLE IF NOT EXISTS slikeOglas(
                        fotoID INTEGER,
                        userID INTEGER,
                        oglasID INTEGER,
                        petID INTEGER,
                        foto TEXT,
                        PRIMARY KEY( fotoID),
                        FOREIGN KEY (userID) REFERENCES Korisnik(userID),
                        FOREIGN KEY (oglasID) REFERENCES Oglas(oglasID),
                        FOREIGN KEY(petID) REFERENCES Ljubimac(petID)
                     );";

                    string createCountPhotosTrigger = @"
                    CREATE TRIGGER IF NOT EXISTS check_numOf_Photos BEFORE INSERT ON slikeOglas WHEN (
                        SELECT COUNT(*) FROM slikeOglas
                        WHERE userID = NEW.userID AND
                        oglasID = NEW.oglasID AND
                        petID= NEW.petID
                        ) >= 3 
                         BEGIN SELECT RAISE(ABORT,'Cannot insert more than three photos of missing pet'); END;
                    ";



                    using (var command = new SQLiteCommand(connection))
                    {
                        command.CommandText = createKorisnikTableQuery;
                        command.ExecuteNonQuery();

                        command.CommandText = createKomunikacijaTableQuery;
                        command.ExecuteNonQuery();

                        command.CommandText = createOglasTableQuery;
                        command.ExecuteNonQuery();

                        command.CommandText = createpisePorTableQuery;
                        command.ExecuteNonQuery();

                        command.CommandText = createslikeOglasTableQuery;
                        command.ExecuteNonQuery();

                        command.CommandText = createLjubimacTableQuery;
                        command.ExecuteNonQuery();

                        command.CommandText = createCountPhotosTrigger;
                        command.ExecuteNonQuery();

                    }
                }
            }
        }

    }
}
