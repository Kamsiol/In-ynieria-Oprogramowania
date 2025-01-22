import pandas as pd
from flask import Flask, render_template
from dataAnalysProcess.processDataFUN import preparationUser, maskSensitiveDUser, preparationBike, preparationOrder, getConnectionString, prepareUserDataForGraphs
from resultPreview.plotCreation import ageBoxplot, agePieChart, userRegion, userSexRegion
from dataAnalysProcess.processDataFUN import loadUserD, mergeUserD, userAge
from resultPreview.plotCreation import userPlot
from dataAnalysProcess.processDataFUN import loadBikeD, mergeBikeD
from resultPreview.plotCreation import bikePlot, aprioriAnalysis

app = Flask(__name__)
connection_string = getConnectionString()

@app.route('/index')
def index():
    return render_template('1index.html')

@app.route('/data_analysis')
def data_analysis():
    rawSamplesUser, processedSamplesUser, processingStepsUser, duplicatesUser, infoUser = preparationUser(connection_string)
    rawSamplesBike, processedSamplesBike, processingStepsBike, duplicatesBike, infoBike = preparationBike(connection_string)
    rawSamplesOrder, duplicatesOrder, infoOrder = preparationOrder(connection_string)
    
    rawSamplesUser['userData'] = maskSensitiveDUser(rawSamplesUser['userData'])
    rawSamplesUser['userAdress'] = maskSensitiveDUser(rawSamplesUser['userAdress'])
    merged = maskSensitiveDUser(processedSamplesUser['merged'])
    processedSamplesUser['cleaned'] = maskSensitiveDUser(processedSamplesUser['cleaned'])

    return render_template(
        '2dataAnalysis.html',
        rawSamplesUser=rawSamplesUser,
        processedSamplesUser=processedSamplesUser,
        processingStepsUser=processingStepsUser,
        duplicatesUser=duplicatesUser,
        infoUser=infoUser,
        merged=merged,
        rawSamplesBike=rawSamplesBike,
        processedSamplesBike=processedSamplesBike,
        processingStepsBike=processingStepsBike,
        duplicatesBike=duplicatesBike,
        infoBike=infoBike,
        rawSamplesOrder=rawSamplesOrder,
        duplicatesOrder=duplicatesOrder,
        infoOrder=infoOrder
    )


@app.route('/user_analysis')
def user_analysis():
    connection_string = getConnectionString()
    df_userData, df_userAddress = loadUserD(connection_string)
    df = mergeUserD(df_userData, df_userAddress)
    df = userAge(df)

    graphs = userPlot(df)
    return render_template('3userAnalysis.html', graphs=graphs)


@app.route('/bike_analysis')
def bike_analysis():
    connection_string = getConnectionString()
    df_modelBike, df_bikeData = loadBikeD(connection_string)
    merged = mergeBikeD(df_modelBike, df_bikeData)
    graphs = bikePlot(merged)
    
    return render_template('4bikeAnalysis.html', graphs=graphs)
    
"""
@app.route('/predictions')
def bike_analysis():
    connection_string = getConnectionString()
    df_modelBike, df_bikeData = loadBikeD(connection_string)  
    fig_bike = aprioriAnalysis(df_bikeData, df_modelBike)
    return render_template('5predictions.html', fig_bike=fig_bike.to_html())

if __name__ == '__main__':
    app.run(debug=True)
    
"""

