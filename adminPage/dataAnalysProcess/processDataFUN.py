from datetime import datetime
import matplotlib.pyplot as plt
import seaborn as sns
import pandas as pd
import os
import glob
import subprocess

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

def getConnectionString(folder_path, prefix="connection_string"):
    try:
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

def cleanD(df, column_name):
    if column_name in df.columns:
        df[column_name] = df[column_name].astype(str).str.strip()  
        df[column_name] = df[column_name].str.replace('"', '', regex=False)  
        df[column_name] = df[column_name].str.lower()  
    else:
        print(f"column '{column_name}' not found in the df")
    return df

def cleanUserD(df):
    text_columns = ['IDuser', 'surnameUser', 'emailUser', 'telnumUser', 'birthdayUser', 'sexUser', 'regionUser', 'cityUser']
    for col in text_columns:
        df = cleanD(df, col)  
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

def ageBoxplot(df):
    # Boxplot для віку загалом
    plt.figure(figsize=(10, 6))
    sns.boxplot(x='ageUser', data=df)
    plt.title('Rozkład wieku (całkowity)', fontsize=16)
    plt.xlabel('Wiek', fontsize=14)
    plt.ylabel('Częstotliwość', fontsize=14)
    plt.show()

    # Додаємо графік з поясненнями для першого Boxplot
    plt.figure(figsize=(10, 6))
    sns.histplot(df['ageUser'], kde=True, bins=20, color='skyblue')
    plt.title('Histogram rozkładu wieku z wykresem gęstości', fontsize=16)  # Заголовок польською
    plt.xlabel('Wiek', fontsize=14)
    plt.ylabel('Częstotliwość', fontsize=14)
    plt.show()

    # Boxplot для віку за регіонами
    plt.figure(figsize=(10, 6))
    sns.boxplot(x='regionUser', y='ageUser', data=df)
    plt.title('Rozkład wieku według regionu', fontsize=16)
    plt.xlabel('Region', fontsize=14)
    plt.ylabel('Wiek', fontsize=14)
    plt.xticks(rotation=90)
    plt.show()

    # Boxplot для віку за статтю
    plt.figure(figsize=(10, 6))
    sns.boxplot(x='sexUser', y='ageUser', data=df)
    plt.title('Rozkład wieku według płci', fontsize=16)
    plt.xlabel('Płeć', fontsize=14)
    plt.ylabel('Wiek', fontsize=14)
    plt.show()

