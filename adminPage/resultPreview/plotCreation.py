import pandas as pd
import matplotlib.pyplot as plt
import seaborn as sns
import datetime as dt
import plotly.express as px
import pandas as pd
from mlxtend.frequent_patterns import apriori, association_rules
import pandas as pd

import sys
import os

sys.path.append(os.path.abspath(os.path.join(os.path.dirname(__file__), "..")))

def ageBoxplot(df):
    if 'ageUser' not in df.columns:
        print("Error: Column 'ageUser' is missing in the dataframe.")
        return None
    fig = px.box(df, x='ageUser', title="Rozkład wieku (całkowity)", labels={'ageUser': 'Wiek'})
    return fig


def agePieChart(df):
    if 'regionUser' not in df.columns:
        print("Error: Column 'regionUser' is missing in the dataframe.")
        return None

    fig = px.pie(df, names='regionUser', title="Odsetek użytkowników w poszczególnych regionach")
    return fig


def userRegion(df):
    if 'regionUser' not in df.columns:
        print("Error: Column 'regionUser' is missing in the dataframe.")
        return None

    region_counts = df['regionUser'].value_counts().reset_index()
    region_counts.columns = ['Region', 'Count']
    fig = px.bar(region_counts, x='Region', y='Count', title="Liczba użytkowników według regionu")
    return fig



def userSexRegion(df):
    if 'regionUser' not in df.columns or 'sexUser' not in df.columns or 'ageUser' not in df.columns:
        print("Błąd: Brakuje wymaganych kolumn ('regionUser', 'sexUser', 'ageUser') w dataframe.")
        return None
    df = df[df['sexUser'] != 'Prefer not to say']

    df['Category'] = df['ageUser'].apply(lambda x: 'Dzieci' if x < 18 else None)
    df.loc[df['ageUser'] >= 18, 'Category'] = df.loc[df['ageUser'] >= 18, 'sexUser']
    df = df[df['Category'].notna()]
    category_counts = df.groupby(['regionUser', 'Category']).size().reset_index(name='Count')


    fig = px.bar(category_counts, 
                 y='regionUser', 
                 x='Count',  
                 color='Category', 
                 barmode='group', 
                 title="Rozkład użytkowników według kategorii w regionach")

    fig.update_layout(
        yaxis_title="Region",
        xaxis_title="Liczba użytkowników",
        title_x=0.5,
        xaxis={'categoryorder': 'total descending'}
    )

    return fig


def userPlot(df):
    graphs = []

    plot_functions = [ageBoxplot, agePieChart, userRegion, userSexRegion]

    for plot_func in plot_functions:
        try:
            fig = plot_func(df)
            if fig is not None:
                graphs.append(fig.to_html(full_html=False))
            else:
                print(f"Warning: {plot_func.__name__} returned None.")
        except Exception as e:
            print(f"Error in {plot_func.__name__}: {e}")

    return graphs


def bikeTypeSummary(df):
    if 'typeModel' not in df.columns or 'amountBike' not in df.columns:
        print("Błąd: Brak wymaganych kolumn ('typeModel', 'amountBike') w dataframe.")
        return None
    type_summary = df.groupby('typeModel')['amountBike'].sum().reset_index()
    fig = px.pie(type_summary, names='typeModel', values='amountBike', 
                 title="Rozkład liczby rowerów według typu modelu",
                 labels={'typeModel': 'Typ roweru', 'amountBike': 'Liczba rowerów'})
    return fig


def bikeTypeRentalComparison(df):
    """
    Tworzenie wykresu porównującego liczbę rowerów dla każdego typu:
    całkowita liczba rowerów oraz liczba rowerów aktualnie wynajmowanych.
    """
    # Sprawdzenie, czy wymagane kolumny istnieją w dataframe
    if 'typeModel' not in df.columns or 'amountBike' not in df.columns or 'amountAvailableBike' not in df.columns:
        print("Błąd: Brakuje wymaganych kolumn ('typeModel', 'amountBike', 'amountAvailableBike') w dataframe.")
        return None

    # Obliczanie liczby rowerów wynajętych
    df['rentedBikes'] = df['amountBike'] - df['amountAvailableBike']

    # Grupowanie danych według typu roweru
    grouped_data = df.groupby('typeModel').agg(
        totalBikes=('amountBike', 'sum'),        # Całkowita liczba rowerów dla każdego typu
        rentedBikes=('rentedBikes', 'sum')      # Liczba wynajętych rowerów dla każdego typu
    ).reset_index()

    # Tworzenie wykresu
    fig = px.bar(
        grouped_data,
        x='typeModel',
        y=['totalBikes', 'rentedBikes'],        # Dwa paski: całkowite i wynajęte rowery
        title="Porównanie całkowitej liczby rowerów i rowerów wynajmowanych według typu",
        labels={
            'value': 'Liczba rowerów',          # Etykieta osi Y
            'typeModel': 'Typ roweru',         # Etykieta osi X
            'variable': 'Status'              # Etykieta legendy
        },
        barmode='group'                         # Grupowanie pasków obok siebie
    )

    # Konfiguracja osi i tytułów
    fig.update_layout(
        xaxis_title="Typ roweru",               # Tytuł osi X
        yaxis_title="Liczba rowerów",           # Tytuł osi Y
        legend_title="Status",                 # Tytuł legendy
        title_x=0.5                             # Wyśrodkowanie tytułu
    )

    return fig

def bikePlot(df):
    graphs = []

    # Список функцій для побудови графіків
    plot_functions = [bikeTypeRentalComparison, bikeTypeSummary]

    # Генерація графіків
    for plot_func in plot_functions:
        try:
            fig = plot_func(df)  # Виклик функції для створення графіка
            if fig is not None:
                graphs.append(fig.to_html(full_html=False))  # Додавання графіка до списку
            else:
                print(f"Warning: {plot_func.__name__} returned None.")
        except Exception as e:
            print(f"Error in {plot_func.__name__}: {e}")

    return graphs

"""

def aprioriAnalysis(df_orders, df_bikes):
    """
    Аналіз залежностей за допомогою алгоритму Apriori.
    """
    # Злиття даних про замовлення та велосипеди
    df = df_orders.merge(df_bikes, on='bikeId')
    
    # Підготовка даних для Apriori: створення матриці "orderId x bikeModel"
    basket = df.groupby(['orderId', 'nameModel'])['bikeId'].count().unstack().fillna(0)
    basket = basket.applymap(lambda x: 1 if x > 0 else 0)

    # Застосування алгоритму Apriori
    frequent_itemsets = apriori(basket, min_support=0.01, use_colnames=True)

    # Генерація правил асоціації
    rules = association_rules(frequent_itemsets, metric="lift", min_threshold=1.0)

    # Фільтрація правил для інтерпретації
    rules = rules.sort_values(by="lift", ascending=False)

    return frequent_itemsets, rules
"""