#functions
from sqlalchemy import create_engine
import pandas as pd

def connectDB(connection_string):
    engine = create_engine(connection_string)
    try:
        conn = engine.connect()
        print("connected DB successfully [readD]")
        return conn  
    except Exception as e:
        print(f"error connect DB [readD] {e}")
        return None
        

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
        df_merged = pd.merge(df_userData, df_userAdress, on='IDuser', how='inner')
        
        selected_columns = ['IDuser', 'surnameUser', 'emailUser', 'telnumUser', 'birthdayUser', 'sexUser', 'regionUser', 'cityUser']
        df_merged = df_merged[selected_columns]
        
        return df_merged
    else:
        print("the df are empty [readD]")
        return None
