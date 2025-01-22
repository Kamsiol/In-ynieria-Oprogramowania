#upload Data about user from .csv to analising process 
import pandas as pd
import pyodbc
import os
import glob
import subprocess

def connectDB(connection_string):
    try:
        conn = pyodbc.connect(connection_string)
        print("DB connect successfully!")
        return conn
    except pyodbc.Error as ex:
        sqlstate = ex.args[0]
        print(f"error num: {sqlstate}")
        print(f"error message: {ex}")
        return None

def closeConnectionDB(cursor, conn):
    try:
        if cursor:
            cursor.close()
        if conn:
            conn.close()
        print("DB connection closed.")
    except Exception as e:
        print(f"Error closing connection: {e}")

def runPS1(script_path):
    try:
        process = subprocess.Popen(
            ["powershell", "-ExecutionPolicy", "Bypass", "-File", script_path],
            stdout=subprocess.PIPE,
            stderr=subprocess.PIPE
        )
        
        # Читаємо результати
        stdout, stderr = process.communicate()
        
        if stderr:
            print(f"Error running PowerShell script: {stderr.decode()}")
            return False
        
        print(f"PowerShell script executed successfully: {stdout.decode()}")
        return True
    
    except Exception as e:
        print(f"Error executing PowerShell script: {e}")
        return False

def getConnectionString(folder_path, prefix="connection_string"):
    try:
        ps_script = os.path.join(folder_path, "connectDBP1.ps1")
        if not runPS1(ps_script):
            print("Failed to run PowerShell script.")
            return None
        files = glob.glob(os.path.join(folder_path, f"{prefix}_*.txt"))
        if not files:
            print("No connection string files found.")
            return None
        
        latest_file = max(files, key=os.path.getmtime)
        with open(latest_file, 'r', encoding='utf-8') as file:
            connection_string = file.read().strip()
            #print(f"Retrieved connection string from: {latest_file}")
            return connection_string
    except Exception as e:
        print(f"An error occurred: {e}")
        return None
 
def deletePS1(folder_path, prefix="connection_string"):
    try:
        files = glob.glob(os.path.join(folder_path, f"{prefix}_*.txt"))
        if not files:
            print("No connection string files found to delete.")
            return
        
        for file in files:
            os.remove(file)
            print(f"Successfully removed file: {file}")
    except Exception as e:
        print(f"Error removing file: {e}")

def readCSV(file_path):
    try:
        data = pd.read_csv(file_path, sep=',', encoding='utf-8', quotechar='"')
        print(f"CSV data loaded from {file_path}")
        return data
    except Exception as e:
        print(f"Error reading CSV: {e}")
        return None

def processDataUserCSV(data):
    data['birthdayUser'] = pd.to_datetime(data['birthdayUser'], format='%d.%m.%Y', errors='coerce')
    if data['birthdayUser'].isnull().any():
        print("Found invalid dates!")
    return data

def processBikeDataCSV(data):
    # Видалення зайвих пробілів у текстових стовпцях
    data['nameModel'] = data['nameModel'].str.strip()
    data['typeModel'] = data['typeModel'].str.strip()

    # Перетворення 'priceModel' у числовий формат (замінюємо кому на крапку)
    data['priceModel'] = data['priceModel'].str.replace(",", ".")

    # Перетворення числових стовпців
    data['amountBike'] = data['amountBike'].astype(int)
    data['amountAvailableBike'] = data['amountAvailableBike'].astype(int)

    return data

def processAdressUserCSV(data):

    data['regionUser'] = data['regionUser'].str.replace('"', '', regex=False) 
    data['cityUser'] = data['cityUser'].str.replace('"', '', regex=False)
    data['streetUser'] = data['streetUser'].str.replace('"', '', regex=False)
    if data.isnull().any().any():
        print("Found missing data!")
        return None

    print("Address data successfully processed.")
    return data

def processSystemDataUserCSV ():
    return 0
 
def DataUser_DB(conn, dataUser):
    try:
        cursor = conn.cursor()
        for index, row in dataUser.iterrows():
            try:
                cursor.execute("""
                    IF NOT EXISTS (SELECT 1 FROM dbo.userData WHERE IDuser = ?)
                    BEGIN
                        INSERT INTO dbo.userData (IDuser, emailUser, telnumUser, passwordUser, nameUser, surnameUser, birthdayUser, sexUser)
                        VALUES (?, ?, ?, ?, ?, ?, ?, ?)
                    END
                """, 
                row['IDuser'], 
                row['IDuser'], 
                row['emailUser'], 
                row['telnumUser'], 
                row['passwordUser'], 
                row['nameUser'], 
                row['surnameUser'], 
                row['birthdayUser'], 
                row['sexUser']
                )
            except Exception as e:
                print(f"Error with the row {index + 1}: {e}")
        conn.commit()
        print("Data successfully inserted into DB.")
    except Exception as e:
        print(f"Error with SQL request: {e}")

def AdressUser_DB(conn, adressData):
    try:
        cursor = conn.cursor()
        for index, row in adressData.iterrows():
            row['regionUser'] = row['regionUser'].strip().replace('"', '') 

            try:
                cursor.execute("""
                    IF NOT EXISTS (SELECT 1 FROM dbo.userAdress WHERE IDuser = ?)
                    BEGIN
                        INSERT INTO dbo.userAdress (IDuser, regionUser, cityUser, postCodeUser, streetUser, numApartUser)
                        VALUES (?, ?, ?, ?, ?, ?)
                    END
                """, 
                row['IDuser'], 
                row['IDuser'], 
                row['regionUser'], 
                row['cityUser'], 
                row['postCodeUser'], 
                row['streetUser'], 
                row['numApartUser']
                )
            except Exception as e:
                print(f"Error inserting row {index + 1}: {row.to_dict()} - {e}")

        conn.commit()
        print("Data successfully inserted into DB.")
    except Exception as e:
        print(f"Critical error with SQL request: {e}")

def ModelBike_DB(conn, modelBike):
    try:
        cursor = conn.cursor()
        for index, row in modelBike.iterrows():
            try:
                if pd.isna(row['IDmodel']) or pd.isna(row['priceModel']) or pd.isna(row['amountBike']) or pd.isna(row['amountAvailableBike']):
                    print(f"Skipping row {index + 1} due to NaN values: {row.to_dict()}")
                    continue  

                cursor.execute("""
                    IF NOT EXISTS (SELECT 1 FROM dbo.modelBike WHERE IDmodel = ?)
                    BEGIN
                        INSERT INTO dbo.modelBike (IDmodel, nameModel, typeModel, priceModel, amountBike, amountAvailableBike)
                        VALUES (?, ?, ?, ?, ?, ?)
                    END
                """,
                row['IDmodel'],              
                row['IDmodel'],              
                row['nameModel'],            
                row['typeModel'],            
                row['priceModel'],           
                row['amountBike'],           
                row['amountAvailableBike']   
                )
            except Exception as e:
                print(f"Error with the row {index + 1}: {e}")
        conn.commit()
        print("Data successfully inserted into DB.")
    except Exception as e:
        print(f"Error with SQL request: {e}")

def BikeData_DB(conn, bikeData):
    try:
        cursor = conn.cursor()
        for index, row in bikeData.iterrows():
            try:

                id_bike = int(row['IDbike'])    
                id_model = int(row['IDmodel'])


                cursor.execute("""
                    IF NOT EXISTS (SELECT 1 FROM dbo.bikeData WHERE IDbike = ?)
                    BEGIN
                        INSERT INTO dbo.bikeData (IDbike, IDmodel)
                        VALUES (?, ?)
                    END
                """, id_bike, id_bike, id_model)

            except Exception as e:
                print(f"Error with the row {index + 1}: {e}")
        
        conn.commit()
        print("Data successfully inserted into DB.")
    except Exception as e:
        print(f"Error with SQL request: {e}")

def OrderUser_DB(conn, orderUser):
    try:
        cursor = conn.cursor()
        for index, row in orderUser.iterrows():
            try:

                id_order = int(row['IDorder']) if pd.notna(row['IDorder']) else None
                id_user = int(row['IDuser']) if pd.notna(row['IDuser']) else None
                id_bike = int(row['IDbike']) if pd.notna(row['IDbike']) else None

                id_payment = 90283091
                cursor.execute("""
                    INSERT INTO dbo.orderUser (IDorder, IDuser, IDbike, IDpayment)
                    VALUES (?, ?, ?, ?)
                """, id_order, id_user, id_bike, id_payment)  

            except Exception as e:
                print(f"Error with the row {index + 1}: {e}")
        
        conn.commit()
        print("Data successfully inserted into DB.")
    except Exception as e:
        print(f"Error with SQL request: {e}")




