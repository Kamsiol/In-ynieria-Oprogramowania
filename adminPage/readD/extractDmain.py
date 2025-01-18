# readD main
import sys
import os

sys.path.append(os.path.abspath(os.path.join(os.path.dirname(__file__), "../adminPage")))


from readD.extractDfun import loadUserD, mergeUserD


def extractDmain(connection_string):
    dfUserData, dfUserAdress = loadUserD(connection_string)
    #print(dfUserData.to_string())
    #print(dfUserAdress.to_string())
    dfMerged = mergeUserD(dfUserData, dfUserAdress)
    print("the task is successfully finished [extractDmain]")
    return dfMerged


if __name__ == "__main__":
    #please put here correct connection_string to your
    #DB to provide the work of the scrypt
    connection_string = "xxxxxxxx"
    dfMerged = extractDmain(connection_string)
    print("completes task successfully [extractDmain]")
    #test the merging operationg, if you need
    #print(dfMerged.to_string(index=True))

