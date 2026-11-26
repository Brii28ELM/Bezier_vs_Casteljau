# Comparacion de Bernstein y De Casteljau

## Parte integrada

La clase `ComparacionCurvas` corresponde a la fase de experimentacion del
proyecto. Compara los dos metodos usando exactamente los mismos puntos de
control y la misma cantidad de muestras.

El boton **Comparar rendimiento (10 veces)** ejecuta cada algoritmo diez veces.
El dibujo no se incluye dentro de los cronometros: solo se mide la llamada
matematica a `GenerarCurva`.

## Metricas

- Tiempo total de Bernstein con `Stopwatch`.
- Tiempo total de De Casteljau con `Stopwatch`.
- Diferencia euclidiana maxima entre muestras correspondientes.
- Diferencia euclidiana promedio entre muestras correspondientes.

Para dos puntos generados en la misma muestra se calcula:

```text
d = raiz((xBernstein - xCasteljau)^2 + (yBernstein - yCasteljau)^2)
```

## Ejemplo de ejecucion

Con los puntos `(100,300)`, `(150,100)`, `(350,100)` y `(400,300)`, y 100
muestras, una ejecucion produjo aproximadamente:

```text
Repeticiones: 10
Muestras: 100
Tiempo Bernstein: 0.163 ms
Tiempo De Casteljau: 0.240 ms
Diferencia maxima: 0.000000000000114
Diferencia promedio: 0.000000000000033
```

Los tiempos cambian entre equipos y ejecuciones. La diferencia numerica debe
permanecer muy cercana a cero porque ambos metodos evaluan la misma curva.

## Complejidad

Si `p` es la cantidad de puntos de control y `m` la cantidad de muestras:

- Bernstein construye sus coeficientes en `O(p)` y evalua cada muestra en
  `O(p)`, por lo que el tiempo total es `O(p + m*p)`.
- De Casteljau realiza `p - 1` niveles de interpolacion por muestra, por lo
  que su tiempo total es `O(m*p^2)`.
- Ambos usan `O(p)` espacio auxiliar y `O(m)` espacio para guardar la curva.

Bernstein calcula los pesos mediante los polinomios de Bernstein. De Casteljau
realiza interpolaciones lineales sucesivas y permite visualizar sus niveles
intermedios. Esta diferencia explica por que Bernstein suele ser mas rapido
cuando hay muchos puntos o muestras, mientras De Casteljau ofrece una
construccion geometrica y una estabilidad numerica muy clara.
