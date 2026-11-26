# Curvas de Bézier mediante polinomios de Bernstein

## Introducción y objetivo

Una curva de Bézier permite describir una trayectoria a partir de un conjunto de puntos de control. El objetivo de esta etapa es implementar su evaluación directa mediante los polinomios de Bernstein y comprobar el resultado con una curva cúbica calculada a mano.

## Fundamentos y fórmulas

Los puntos de control determinan la forma de la curva. La curva pasa por el primero y el último. Los puntos intermedios influyen en su forma. Con n + 1 puntos se evalúa la fórmula de grado n. Dos puntos dan una curva lineal, tres una cuadrática y cuatro una cúbica.

El parámetro t toma valores entre 0 y 1. En t = 0 se obtiene el punto inicial y en t = 1 el final. Un incremento fijo de t produce muestras uniformes en el parámetro, aunque la distancia entre ellas puede variar.

Un polinomio de Bernstein da el peso de un punto de control. En el intervalo [0, 1] los pesos son no negativos y suman 1. Las coordenadas se calculan por separado con los mismos pesos.

```text
B(i,n,t) = C(n,i) * t^i * (1-t)^(n-i)
P(t) = suma de B(i,n,t) * Pi, con i de 0 a n

C(n,0) = 1
C(n,i) = C(n,i-1) * (n-i+1) / i
```

## Cálculo manual de la curva cúbica

Se usan P0 = (100, 300), P1 = (150, 100), P2 = (350, 100) y P3 = (400, 300). Por la fórmula multiplicativa, los coeficientes son 1, 1·3/1 = 3, 3·2/2 = 3 y 3·1/3 = 1. Para cada valor de t se evalúan los cuatro pesos y se sustituyen en las dos sumas.

```text
B0 = (1-t)^3
B1 = 3t(1-t)^2
B2 = 3t^2(1-t)
B3 = t^3

X = 100B0 + 150B1 + 350B2 + 400B3
Y = 300B0 + 100B1 + 100B2 + 300B3
```

| t | Sustitución en (B0, B1, B2, B3) | Cálculo de X | Cálculo de Y | Resultado (X, Y) |
| --- | --- | --- | --- | --- |
| 0 | (1³, 0, 0, 0) = (1, 0, 0, 0) | 100·1 = 100 | 300·1 = 300 | (100, 300) |
| 0,25 | ((3/4)³, 3(1/4)(3/4)², 3(1/4)²(3/4), (1/4)³) = (27, 27, 9, 1)/64 | (2700 + 4050 + 3150 + 400)/64 = 160,9375 | (8100 + 2700 + 900 + 300)/64 = 187,5 | (160,9375; 187,5) |
| 0,5 | ((1/2)³, 3(1/2)³, 3(1/2)³, (1/2)³) = (1, 3, 3, 1)/8 | (100 + 450 + 1050 + 400)/8 = 250 | (300 + 300 + 300 + 300)/8 = 150 | (250, 150) |
| 0,75 | ((1/4)³, 3(3/4)(1/4)², 3(3/4)²(1/4), (3/4)³) = (1, 9, 27, 27)/64 | (100 + 1350 + 9450 + 10800)/64 = 339,0625 | (300 + 900 + 2700 + 8100)/64 = 187,5 | (339,0625; 187,5) |
| 1 | (0, 0, 0, 1³) = (0, 0, 0, 1) | 400·1 = 400 | 300·1 = 300 | (400, 300) |

Para comparar con De Casteljau, se pueden usar estos mismos cuatro puntos en el mismo orden y cinco muestras. Así se obtienen los mismos valores de t.

## Implementación y contrato compartido

- `Bernstein.EvaluarPunto(List<Punto2D> puntosControl, double t)` devuelve un `Punto2D` nuevo. Requiere al menos un punto y un valor de t en [0, 1].
- `Bernstein.GenerarCurva(List<Punto2D> puntosControl, int cantidadMuestras)` devuelve una `List<Punto2D>` ordenada desde t = 0 hasta t = 1. Requiere al menos dos puntos y dos muestras. La cantidad incluye los extremos y para la muestra i se usa `(double)i / (cantidadMuestras - 1)`.
- Ambos métodos conservan las coordenadas en `double` y rechazan argumentos inválidos. Los puntos de entrada se conservan. En los extremos se copian las coordenadas del primer o del último punto de control.

Se calcula una vez el arreglo de coeficientes por llamada. Al generar varias muestras se reutiliza el mismo arreglo. Para cada muestra interior se recorren los puntos, se calculan sus pesos con `Math.Pow` y se suman las contribuciones a X e Y.

## Ventajas, desventajas y estabilidad

La evaluación directa se parece a la fórmula matemática y permite calcular cualquier muestra a partir de t. La relación multiplicativa evita construir factoriales grandes y simplifica el cálculo de los coeficientes.

El cálculo tiene errores de redondeo por usar `double`. En grados muy altos, algunas potencias pueden redondearse a cero antes de multiplicarse por el coeficiente y los coeficientes centrales pueden superar el rango de `double`. En este último caso, el programa muestra un mensaje. Esta limitación corresponde a la forma de evaluar la expresión por separado. La devolución explícita de los extremos conserva sus coordenadas exactas.

## Complejidad

Si p es el número de puntos de control, el arreglo de coeficientes se construye en O(p) tiempo y ocupa O(p) espacio. La evaluación interior recorre p pesos, con dos llamadas a `Math.Pow` por peso. Contando cada operación de punto flotante de tamaño fijo como una operación, evaluar un punto cuesta O(p) tiempo y O(p) espacio auxiliar en este código.

Con m muestras, el costo es O(p + mp) en el peor caso. El espacio auxiliar es O(p) y el resultado ocupa O(m). Estas cuentas corresponden al módulo matemático y dejan fuera el dibujo.

## Interfaz gráfica

En el formulario se ingresan X e Y, se agregan puntos y se selecciona una fila para cargar sus coordenadas y modificarla o eliminarla. El origen está arriba a la izquierda del panel y el eje Y aumenta hacia abajo. La cantidad de puntos y el grado se actualizan con cada edición. El control de muestras acepta entre 2 y 10 000 y empieza en 100.

El panel usa el evento `Paint` para mostrar puntos etiquetados, el polígono gris y la curva azul formada por segmentos entre las muestras. Cambiar puntos o muestras elimina el resultado anterior. Con **Generar curva** se vuelve a calcular y con **Limpiar** se borran los puntos y el dibujo.

## Pruebas funcionales

La solución compiló con .NET 10 y se ejecutó una comprobación temporal sin dependencias externas.

| Prueba | Resultado observado |
| --- | --- |
| Evaluar la cúbica para t = 0, 0,25, 0,5, 0,75 y 1 | Los valores de ambos métodos coinciden con la tabla con tolerancia de 10⁻¹⁰. |
| Generar curvas de grados 1, 2, 3 y 4 | Se comprobó la cantidad de muestras solicitada y la coincidencia de ambos extremos. |
| Agregar, seleccionar, modificar y eliminar en el formulario | Se actualizaron la lista, las coordenadas de entrada y los indicadores. La eliminación reordenó las etiquetas. |
| Cambiar de 100 a 5 y luego a 2 muestras | El botón generó las cantidades solicitadas. Una edición o cambio de muestras borró la curva previa. |
| Dibujar y limpiar | El panel produjo segmentos azules, segmentos grises y marcas negras. Limpiar dejó vacías las listas y actualizó los indicadores. |
| Enviar argumentos inválidos al módulo matemático | Se rechazaron una lista vacía para generar, una muestra y t igual a NaN. |

Para revisar manualmente la interfaz en Visual Studio, pulsa **F5**, ingresa los cuatro puntos de la tabla y genera con cinco muestras. También quedan como comprobaciones manuales los mensajes de los botones al intentar modificar o eliminar sin selección o generar con menos de dos puntos. Los campos numéricos restringen coordenadas y muestras según sus rangos configurados.

## Conclusiones parciales

La evaluación directa reprodujo las cinco coordenadas calculadas a mano. La misma fórmula permite cambiar el grado añadiendo o quitando puntos. La representación `Punto2D`, los valores de t de la tabla y el criterio de muestreo quedan disponibles para trabajar con los mismos datos en los siguientes módulos.

## Referencias

- Farouki, R. T. (2012). *The Bernstein polynomial basis: A centennial retrospective*. Computer Aided Geometric Design, 29(6), 379–419. [DOI](https://doi.org/10.1016/j.cagd.2012.03.001)
- Microsoft. *Control.Paint Event*. [Referencia de Windows Forms](https://learn.microsoft.com/dotnet/api/system.windows.forms.control.paint)
