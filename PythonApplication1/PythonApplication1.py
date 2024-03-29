import numpy as np
from scipy import stats

# Dados da série
data = [27.8, 28.2, 31.7, 26.9, 28.5, 28.8, 26.3, 27.4, 25.8, 27.2, 27.4, 29.4, 30.3, 32.6, 33.6, 27.2, 29.7, 32.2, 30.5, 32
        , 33.4, 34.2, 34.4, 34.5, 32.5, 30.9, 31.5, 28.7, 23.7, 21.8, 26.2, 27, 23.1, 29.7, 29.8, 30.3, 29.2, 29.7, 30.2, 29.8
        , 30.1, 31.3, 30.8, 31.1, 33.5, 33.2, 31.4, 30.5, 28.5, 30.6, 30.1, 31.3, 33.3, 33.2, 34, 33.3, 32.7, 33.9, 34.4, 31.8
        , 29.6, 31.6, 31.5, 33.6, 32.5, 33.3, 33.7, 32.8, 33.6, 31.5, 28.1, 26.7, 28.2, 27.9, 30.1, 32.6, 33.5, 29.7, 25.3, 27.3
        , 28.8, 30.1, 31.7, 27.8, 28, 26.2, 30, 31.1, 31.9, 30.2, 22.3, 27.1, 30.2, 31.6, 31.8, 29.9, 29.8, 31.5, 31.2, 31.1, 30.2
        , 29.5, 22.8, 27.5, 25.2, 24.5, 25.3, 26.3, 28.5, 28.6, 28.2, 30.1, 23.9, 29.3, 30.8, 30.6, 30.8, 30.8, 30.3, 27.8, 25.5
        , 30.6, 30.9, 24.5, 20, 24.3, 25.9, 26, 26.8, 27.6, 25.3, 24.1, 26, 26.7, 22.4, 23.2, 16.7, 11.5, 18, 19.1, 21.6, 23.7, 25.9
        , 26.7, 26.3, 25.3, 26, 27.2, 28.4, 27.9, 19.6, 24.9, 28.1, 21.1, 22.4, 25, 26.3, 27.3, 25.5, 21.3, 22.4, 20.7, 19.6, 19.3, 20.1
        , 23.2, 26.7, 28.2, 22.3, 21.8, 22, 27, 27.3, 27.8, 27, 27.2, 22.3, 24, 25.2, 25.5, 25.2, 25.8, 28, 28, 26.8, 25.9, 25.7, 28.1
        , 27.3, 26.4, 26.5, 27.1, 25.5, 23.3, 27, 28.9, 28.4, 29.9, 28.3, 28.2, 26.9, 27.5, 28, 28.2, 28.7, 28.9, 28.7, 28, 28.8, 23.6
        , 23.3, 26.5, 29, 29.5, 30.8, 30.9, 30.4, 26.8, 22, 22.5, 20.3, 17.9, 19.9, 23.8, 27.9, 30, 30, 31.3, 20.2, 19.5, 16.2, 20.3, 20.5
        , 24.9, 26.2, 28.3, 29, 30.8, 31.2, 32.8, 21.4, 22.3, 25.8, 28.3, 30.8, 31.7, 22.8, 25.6, 22, 23.9, 31.9, 34.1, 33.9, 27.4, 27.3
        , 30.2, 21.4, 18.4, 24.2, 24.3, 28.2, 32.1, 27.3, 22.4, 20.7, 22.9, 26.1, 29, 18.9, 23.8, 19, 22.2, 17.3, 28.2, 29.1, 23.8, 29.2
        , 32.2, 33.1, 26.1, 28.4, 29.3, 26.8, 28.9, 23.7, 30.3, 33.4, 27.5, 31.8, 32.5, 23.7, 26.9, 29.9, 26.7, 29.8, 31.4, 31.9, 30.8
        , 33.5, 35.2, 32.6, 33.9, 31.1, 24.9, 23, 20.4, 23.1, 25.3, 26.1, 25.2, 28.3, 29.2, 31.5, 34, 27.3, 31.5, 32.4, 29.6, 31.5, 32.3
        , 31.4, 31.9, 33.1, 32.6, 33.3, 30.3, 30.3, 30.6, 29.3, 30.6, 30.1, 29.2, 31.3, 28.6, 29.3, 28.7, 28.1, 27.9, 24.5, 26, 30.1, 32.7
        , 35, 34.8, 31.2, 28.1, 26.7, 26.2, 30.6, 30.9, 31.5, 31.8, 24.8, 30, 28.2, 28.5, 31.5, 30.9, 31.3, 27.6, 29.1, 28.4, 27.8, 29.6, 30]

# Adicionar um valor constante para contornar o erro "data must be positive"
constant = 0.00
transformed_data, lambda_ = stats.boxcox(np.array(data))

print("Valor estimado de lambda:", lambda_)
print("Valores transformados:", transformed_data)
