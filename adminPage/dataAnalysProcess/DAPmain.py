import sys
import os

sys.path.append(os.path.abspath(os.path.join(os.path.dirname(__file__), "..")))

from dataAnalysProcess.processDataFUN import loadUserD, mergeUserD
from dataAnalysProcess.processDataFUN import getConnectionString, userAge, cleanUserD, deletePS1
from dataAnalysProcess.processDataFUN import preparationUser
from resultPreview.plotCreation import userPlot

if __name__ == "__main__":
    
    connection_string = getConnectionString()
    print(connection_string)
    df_userData, df_userAdress = loadUserD(connection_string)
    dfDataUser = mergeUserD(df_userData, df_userAdress)
    dfDataUser = userAge(dfDataUser)
    userPlot(dfDataUser)
    deletePS1()