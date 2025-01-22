from datetime import datetime
import matplotlib.pyplot as plt
import seaborn as sns
import pandas as pd
import os
import glob
import subprocess
from sqlalchemy import create_engine

def runPS1(script_path):
    try:
        process = subprocess.Popen(
            ["powershell", "-ExecutionPolicy", "Bypass", "-File", script_path],
            stdout=subprocess.PIPE,
            stderr=subprocess.PIPE
        )
        stdout, stderr = process.communicate()
        if stderr:
            print(f"Error running PowerShell script: {stderr.decode()}")
            return False
        
        print(f"PowerShell script executed successfully: {stdout.decode()}")
        return True
    except Exception as e:
        print(f"Error executing PowerShell script: {e}")
        return False

def getConnectionString(prefix="connection_string"):
    try:
        folder_path = "." 
        ps_script = os.path.join(folder_path, "connectDBP.ps1")
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

def deletePS1( prefix="connection_string"):
    try:
        folder_path = "." 
        files = glob.glob(os.path.join(folder_path, f"{prefix}_*.txt"))
        if not files:
            print("No connection string files found to delete.")
            return
        for file in files:
            os.remove(file)
            print(f"Successfully removed file: {file}")
    except Exception as e:
        print(f"Error removing file: {e}")


def connectDB(connection_string):
    engine = create_engine(connection_string)
    try:
        conn = engine.connect()
        print("connected DB successfully [readD]")
        return conn  
    except Exception as e:
        print(f"error connect DB [readD] {e}")
        return None    

def cleanD(df, column_name):
    if column_name in df.columns:
        df[column_name] = df[column_name].astype(str).str.strip()  
        df[column_name] = df[column_name].str.replace('"', '', regex=False)  
        df[column_name] = df[column_name].str.lower()  
    else:
        print(f"column '{column_name}' not found in the df")
    return df

def userAge(df):
    if 'birthdayUser' in df.columns:
        today = datetime.today()
        
        def calculate_age(birthdate):
            try:
                if pd.isna(birthdate):
                    return None
                birthdate = datetime.strptime(str(birthdate), '%Y-%m-%d')
                age = today.year - birthdate.year
                if today.month < birthdate.month or (today.month == birthdate.month and today.day < birthdate.day):
                    age -= 1
                return age
            except Exception as e:

                print(f"error calculating age for {birthdate}: {e}")
                return None  
            
        df['ageUser'] = df['birthdayUser'].apply(calculate_age)
    else:
        print("column 'birthdayUser' not found in the df [processDataFUN]")
    df = df.drop(columns=['birthdayUser'])
    return df

def loadUserD(connection_string):
    conn = connectDB(connection_string)
    if conn is not None:
        query_userData = "SELECT * FROM [DB_IOPROJECT].[dbo].[userData];"
        query_userAdress = "SELECT * FROM [DB_IOPROJECT].[dbo].[userAdress];"
    
        df_userData = pd.read_sql(query_userData, conn)
        df_userAdress = pd.read_sql(query_userAdress, conn)
        
        conn.close() 
        return df_userData, df_userAdress
    else:
        return None, None

def mergeUserD(df_userData, df_userAdress):
    if df_userData is not None and df_userAdress is not None:
        dfMerged = pd.merge(df_userData, df_userAdress, on='IDuser', how='inner')
        
        selected_columns = ['IDuser', 'surnameUser', 'emailUser', 'telnumUser', 'birthdayUser', 'sexUser', 'regionUser', 'cityUser']
        dfMerged = dfMerged[selected_columns]
        
        return dfMerged
    else:
        print("the df are empty [readD]")
        return None
    
def cleanUserD(df):
    text_columns = ['IDuser', 'surnameUser', 'emailUser', 'telnumUser', 'birthdayUser', 'sexUser', 'regionUser', 'cityUser']
    for col in text_columns:
        df = cleanD(df, col)  
    return df

def maskSensitiveDUser(data):
    """
    Функція для маскування чутливих даних (телефони, email, пароль, ім'я) у списку словників.
    """
    if isinstance(data, list):
        for row in data:
                        
            if 'IDuser' in row and isinstance(row['IDuser'], str):
                row['IDuser'] = row['IDuser'][:1] + "****" + row['telnumUser'][-1:]

            # Маскування email
            if 'emailUser' in row and isinstance(row['emailUser'], str):
                row['emailUser'] = row['emailUser'][:3] + "****" + row['emailUser'][-3:]
            
            # Маскування телефону
            if 'telnumUser' in row and isinstance(row['telnumUser'], str):
                row['telnumUser'] = row['telnumUser'][:3] + "****" + row['telnumUser'][-3:]
            
            # Маскування паролю
            if 'passwordUser' in row and isinstance(row['passwordUser'], str):
                row['passwordUser'] = "********"
            
            # Маскування імені (залишаємо першу літеру)
            if 'nameUser' in row and isinstance(row['nameUser'], str):
                row['nameUser'] = row['nameUser'][0] + "****"
            
            if 'surnameUser' in row and isinstance(row['surnameUser'], str):
                row['surnameUser'] = row['surnameUser'][0] + "****"    
    return data

def count_rows(df):
    return len(df)

def preparationUser(connection_string):
    # Завантаження даних
    dfUserData, dfUserAdress = loadUserD(connection_string)
    
    rawSamplesUser = {
        'userData': dfUserData.head(3).to_dict(orient='records'),
        'userAdress': dfUserAdress.head(3).to_dict(orient='records'),
    }

    # Злиття та очищення даних
    dfMerged = mergeUserD(dfUserData, dfUserAdress)
    dfData = cleanUserD(dfMerged)
    dfData = userAge(dfData)
    

    processedSamplesUser = {
        'merged': dfMerged.head(3).to_dict(orient='records'),
        'cleaned': dfData.head(3).to_dict(orient='records'),
    }

    processingStepsUser = [
        "Dodano wiek użytkownika (ageUser), usunięto dataUrodzeniaUser.",
        "Przetworzono dane tekstowe: usunięto spacje, cudzysłowy oraz przekształcono na małe litery.",
        "Połączono tabele userData i userAdress w celu przyspieszenia analizy danych."
    ]

    duplicatesUser = {
        'userData': dfUserData.duplicated().sum(),
        'userAdress': dfUserAdress.duplicated().sum(),
    }

    infoUser = {
        'countUser': count_rows(dfUserData),
        'countAdress': count_rows(dfUserAdress),
        'countMerged': count_rows(dfMerged),
    }

    return rawSamplesUser, processedSamplesUser, processingStepsUser, duplicatesUser, infoUser

def loadBikeD(connection_string):
    """
    Завантажує дані з таблиць modelBike та bikeData.
    """
    conn = connectDB(connection_string)
    if conn is not None:
        query_modelBike = "SELECT * FROM [DB_IOPROJECT].[dbo].[modelBike];"
        query_bikeData = "SELECT * FROM [DB_IOPROJECT].[dbo].[bikeData];"
    
        df_modelBike = pd.read_sql(query_modelBike, conn)
        df_bikeData = pd.read_sql(query_bikeData, conn)
        
        conn.close()
        return df_modelBike, df_bikeData
    else:
        return None, None

def mergeBikeD(df_modelBike, df_bikeData):
    """
    Поєднує дані з таблиць modelBike та bikeData.
    """
    if df_modelBike is not None and df_bikeData is not None:
        df_merged = pd.merge(df_modelBike, df_bikeData, on='IDmodel', how='inner')
        
        # Вибираємо лише потрібні колонки
        selected_columns = ['IDbike', 'IDmodel', 'typeModel', 
                            'priceModel', 'amountBike', 'amountAvailableBike']
        df_merged = df_merged[selected_columns]
        
        return df_merged
    else:
        print("The dataframes are empty [readD]")
        return None

def cleanBikeD(df):
    """
    Очищає текстові колонки від зайвих символів, пробілів та приводить до нижнього регістру.
    """
    text_columns = ['IDbike', 'IDmodel', 'nameModel', 'typeModel']
    for col in text_columns:
        df = cleanD(df, col)
    return df

def preparationBike(connection_string):
    """
    Головна функція для обробки даних про байки.
    """
    # Завантаження даних
    dfModelBike, dfBikeData = loadBikeD(connection_string)
    
    rawSamplesBike = {
        'modelBike': dfModelBike.head(3).to_dict(orient='records'),
        'bikeData': dfBikeData.head(3).to_dict(orient='records'),
    }

    # Злиття та очищення даних
    dfMerged = mergeBikeD(dfModelBike, dfBikeData)
    dfCleaned = cleanBikeD(dfMerged)

    processedSamplesBike = {
        'merged': dfMerged.head(3).to_dict(orient='records'),
        'cleaned': dfCleaned.head(3).to_dict(orient='records'),
    }

    processingStepsBike = [
        "Przetworzono dane tekstowe: usunięto spacje, cudzysłowy oraz przekształcono na małe litery.",
        "Połączono tabele modelBike i bikeData w celu ułatwienia analizy danych.",
    ]

    duplicatesBike = {
        'modelBike': dfModelBike.duplicated().sum(),
        'bikeData': dfBikeData.duplicated().sum(),
    }
        
    infoBike = {
        'countBike': count_rows(dfBikeData),
        'countModel': count_rows(dfModelBike),
        'countMerged': count_rows(dfMerged),
    }

    return rawSamplesBike, processedSamplesBike, processingStepsBike, duplicatesBike, infoBike

def loadOrderD(connection_string):
    """
    Завантажує дані з таблиць modelBike та bikeData.
    """
    conn = connectDB(connection_string)
    if conn is not None:
        query_orderUser = "SELECT * FROM [DB_IOPROJECT].[dbo].[orderUser];"
    
        orderUser = pd.read_sql(query_orderUser, conn)
        
        conn.close()
        return orderUser
    else:
        return None

def preparationOrder(connection_string):
    dfOrderUser = loadOrderD(connection_string) 
    rawSamplesOrder = {
        'orderUser': dfOrderUser.head(3).to_dict(orient='records'),
    }

    duplicatesOrder = {
        'orderUser': dfOrderUser.duplicated().sum(), 
    }

    infoOrder = {
        'countUser': count_rows(dfOrderUser),
    }

    return rawSamplesOrder, duplicatesOrder, infoOrder

def prepareUserDataForGraphs(df):
    # Перевірка на наявність необхідних колонок
    if 'ageUser' not in df.columns or 'sexUser' not in df.columns:
        print("Missing required columns for graph preparation")
        return None

    # Фільтрація та підготовка даних
    df = df.dropna(subset=['ageUser'])  # Видалення пропусків у віці
    df['ageUser'] = df['ageUser'].astype(int)  # Переведення віку в цілі числа

    # Побудова графіка
    fig = px.histogram(df, x="ageUser", color="sexUser", title="Age Distribution by Sex")
    
    # Повернення графіка у форматі HTML
    return fig.to_html(full_html=False)


