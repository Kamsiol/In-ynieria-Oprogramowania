import sys
import os

sys.path.append(os.path.abspath(os.path.join(os.path.dirname(__file__), "..")))

from CSVdb.extractCSVfun import connectDB, closeConnectionDB, readCSV, processDataUserCSV, processAdressUserCSV, processBikeDataCSV, AdressUser_DB, DataUser_DB, ModelBike_DB, getConnectionString, deletePS1

def UserProcess():
    dataUser_path = "C:/Users/Mariia/Desktop/University/sem 5/In-ynieria-Oprogramowania/DB/csvDATA/dataUser.csv"
    adressUser_path = "C:/Users/Mariia/Desktop/University/sem 5/In-ynieria-Oprogramowania/DB/csvDATA/adressUser.csv"
    modelBike_path = "C:/Users/Mariia/Desktop/University/sem 5/In-ynieria-Oprogramowania/DB/csvDATA/modelBike.csv"
    global dataUser, adressData, modelBike


    folder_path = "." 
    connection_string = getConnectionString(folder_path)

    if connection_string:
        print("Connection String:", connection_string)


    dataUser = readCSV(dataUser_path)
    adressData = readCSV(adressUser_path)
    modelBike = readCSV(modelBike_path)

    dataUser = processDataUserCSV(dataUser)
    adressData = processAdressUserCSV(adressData)
 
    conn = connectDB(connection_string)
    if not conn:
        return 
   
    DataUser_DB(conn, dataUser)
    AdressUser_DB(conn, adressData)
    ModelBike_DB(conn, modelBike)
    
    closeConnectionDB(None, conn)
    deletePS1(folder_path)





if __name__ == "__main__":
    UserProcess()
