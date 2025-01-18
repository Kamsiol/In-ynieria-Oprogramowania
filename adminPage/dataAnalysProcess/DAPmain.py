import sys
import os

sys.path.append(os.path.abspath(os.path.join(os.path.dirname(__file__), "..")))

from readD.extractDmain import extractDmain
from dataAnalysProcess.processDataFUN import getConnectionString, userAge, cleanUserD, ageBoxplot, deletePS1

def DAPmain(dfData):
    dfData = cleanUserD(dfData)
    dfData = userAge(dfData)
    #print(dfData.info())
    #ageBoxplot(dfData)
    
    return dfData


if __name__ == "__main__":

    folder_path = "." 
    connection_string = getConnectionString(folder_path)
    dfData = extractDmain(connection_string)
    dfData = DAPmain(dfData)
    deletePS1(folder_path)