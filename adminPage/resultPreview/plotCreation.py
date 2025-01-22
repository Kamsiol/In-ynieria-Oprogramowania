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
