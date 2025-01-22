import pandas as pd
from flask import Flask, render_template
from dataAnalysProcess.processDataFUN import preparationUser, maskSensitiveDUser, preparationBike, preparationOrder, getConnectionString, prepareUserDataForGraphs
from resultPreview.plotCreation import ageBoxplot, agePieChart, userRegion, userSexRegion
from dataAnalysProcess.processDataFUN import loadUserD, mergeUserD, userAge
from resultPreview.plotCreation import userPlot

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

    

"""
@app.route('/user_analysis')
def user_analysis():
    fig_age = analyze_user_data(dataUser)
    return render_template('user_analysis.html', fig_age=fig_age.to_html())

@app.route('/bike_analysis')
def bike_analysis():
    fig_bike = analyze_bike_data(bikeData, modelBike)
    return render_template('bike_analysis.html', fig_bike=fig_bike.to_html())

"""
if __name__ == '__main__':
    app.run(debug=True)

