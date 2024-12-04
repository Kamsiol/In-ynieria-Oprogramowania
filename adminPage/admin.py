import pandas as pd
import plotly.express as px

# Шляхи до файлів
dataUser_path = "C:/Users/Mariia/Desktop/University/sem 5/In-ynieria-Oprogramowania/DB/csvDATA/dataUser.csv"
adressUser_path = "C:/Users/Mariia/Desktop/University/sem 5/In-ynieria-Oprogramowania/DB/csvDATA/adressUser.csv"

# Читання даних
dataUser = pd.read_csv(dataUser_path, sep=',', encoding='utf-8', quotechar='"')
adressUser = pd.read_csv(adressUser_path, sep=',', encoding='utf-8', quotechar='"')

# Обчислення віку
dataUser['birthdayUser'] = pd.to_datetime(dataUser['birthdayUser'], format='%d.%m.%Y')
dataUser['age'] = (pd.to_datetime('today') - dataUser['birthdayUser']).dt.days // 365

# Злиття двох датафреймів за IDuser
merged_data = pd.merge(dataUser, adressUser, on='IDuser')

# Розподіл віку користувачів
fig_age = px.histogram(dataUser, x='age', nbins=20, title="Розподіл віку користувачів")
fig_age.update_layout(xaxis_title='Вік', yaxis_title='Кількість користувачів')

# Розподіл за статтю
fig_sex = px.histogram(dataUser, x='sexUser', title="Розподіл користувачів за статтю")
fig_sex.update_layout(xaxis_title='Стать', yaxis_title='Кількість користувачів')

# Розподіл за регіоном
fig_region = px.histogram(adressUser, x='regionUser', title="Розподіл користувачів за регіоном")
fig_region.update_layout(xaxis_title='Регіон', yaxis_title='Кількість користувачів')


# Збереження графіків як HTML
fig_age.write_html("age_distribution.html")
fig_sex.write_html("sex_distribution.html")
fig_region.write_html("region_distribution.html")

print("Графіки збережено як HTML файли.")
