#creating plots for user data 
import pandas as pd
import matplotlib.pyplot as plt
import seaborn as sns
import datetime as dt
import plotly.express as px
import pandas as pd

import sys
import os

sys.path.append(os.path.abspath(os.path.join(os.path.dirname(__file__), "..")))

def ageBoxplot(df):
    # Графік для віку загалом
    fig = px.box(df, x='ageUser', title="Rozkład wieku (całkowity)", labels={'ageUser': 'Wiek'})
    fig.show()

    # Додаємо графік з поясненнями для першого Boxplot
    fig_hist = px.histogram(df, x="ageUser", nbins=20, title="Histogram rozkładu wieku z wykresem gęstości")
    fig_hist.update_layout(xaxis_title="Wiek", yaxis_title="Częstotliwość")
    fig_hist.show()

    # Boxplot для віку за регіонами
    fig_region = px.box(df, x='regionUser', y='ageUser', title="Rozkład wieku według regionu")
    fig_region.update_layout(xaxis_title="Region", yaxis_title="Wiek")
    fig_region.show()

    # Boxplot для віку за статтю
    fig_sex = px.box(df, x='sexUser', y='ageUser', title="Rozkład wieku według płci")
    fig_sex.update_layout(xaxis_title="Płeć", yaxis_title="Wiek")
    fig_sex.show()

def agePieChart(df):
    fig = px.pie(df, names='regionUser', title="Odsetek użytkowników w poszczególnych regionach")
    fig.show()

def userRegion(df):
    fig = px.bar(df['regionUser'].value_counts().reset_index(), x='index', y='regionUser',
                 title="Liczba użytkowników według regionu")
    fig.update_layout(xaxis_title="Region", yaxis_title="Liczba użytkowników")
    fig.show()

def userSexRegion(df):
    df['ageUser'] = df['ageUser'].apply(lambda x: 'Dzieci' if x < 18 else ('Mężczyzna' if x % 2 == 0 else 'Kobieta'))  # Спрощене для прикладу
    fig = px.bar(df, x="regionUser", color="sexUser", title="Rozkład użytkowników według płci w regionach")
    fig.update_layout(xaxis_title="Region", yaxis_title="Liczba użytkowników")
    fig.show()

def userPlot(df):
    ageBoxplot(df)
    agePieChart(df)
    userRegion(df)
    userSexRegion(df)
