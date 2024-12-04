import pandas as pd
import matplotlib.pyplot as plt
import seaborn as sns

dataUser_path = "C:/Users/Mariia/Desktop/University/sem 5/In-ynieria-Oprogramowania/DB/csvDATA/dataUser.csv"
adressUser_path = "C:/Users/Mariia/Desktop/University/sem 5/In-ynieria-Oprogramowania/DB/csvDATA/adressUser.csv"

dataUser = pd.read_csv(dataUser_path, sep=',', encoding='utf-8', quotechar='"')
adressUser = pd.read_csv(adressUser_path, sep=',', encoding='utf-8', quotechar='"')

dataUser['birthdayUser'] = pd.to_datetime(dataUser['birthdayUser'], format='%d.%m.%Y')
dataUser['age'] = (pd.to_datetime('today') - dataUser['birthdayUser']).dt.days // 365

merged_data = pd.merge(dataUser, adressUser, on='IDuser')
merged_data['postCodeUser_numeric'] = pd.to_numeric(merged_data['postCodeUser'], errors='coerce')

# age
plt.figure(figsize=(10, 6))
sns.histplot(dataUser['age'], bins=20, kde=True, color='blue')
plt.title('Розподіл віку користувачів')
plt.xlabel('Вік')
plt.ylabel('Кількість користувачів')
plt.show()

# sex
plt.figure(figsize=(8, 6))
sns.countplot(x='sexUser', data=dataUser, palette='Set2')
plt.title('Розподіл користувачів за статтю')
plt.xlabel('Стать')
plt.ylabel('Кількість користувачів')
plt.show()

# region
plt.figure(figsize=(8, 6))
sns.countplot(x='regionUser', data=adressUser, palette='Set3')
plt.title('Розподіл користувачів за регіоном')
plt.xlabel('Регіон')
plt.ylabel('Кількість користувачів')
plt.xticks(rotation=45)
plt.show()

# age+postCode
correlation = merged_data[['age', 'postCodeUser_numeric']].corr()
sns.heatmap(correlation, annot=True, cmap='coolwarm', vmin=-1, vmax=1)
plt.title('Кореляція між віком та поштовим індексом')
plt.show()
