#upload Data about user from .csv to analising process 
import pandas as pd
import matplotlib.pyplot as plt
import seaborn as sns

dataUser_path = "C:/Users/Mariia/Desktop/University/sem 5/In-ynieria-Oprogramowania/DB/csvDATA/dataUser.csv"
adressUser_path = "C:/Users/Mariia/Desktop/University/sem 5/In-ynieria-Oprogramowania/DB/csvDATA/adressUser.csv"

#systemDataUser_path = "C:/Users/Mariia/Desktop/Universitysem 5/In-ynieria-Oprogramowania/DB/csvDATA/systemDataUser.csv"

dataUser = pd.read_csv(dataUser_path, sep=',', encoding='utf-8', quotechar='"')
print(dataUser.head())

adressUser = pd.read_csv(adressUser_path, sep=',', encoding='utf-8', quotechar='"')
print(adressUser.head())


merged_data = pd.merge(dataUser, adressUser, on='IDuser')
print(merged_data.head())
