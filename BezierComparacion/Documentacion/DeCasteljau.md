# Evaluación de curvas de Bézier con De Casteljau

De Casteljau evalúa una curva de Bézier mediante interpolaciones lineales sucesivas. Los niveles intermedios muestran cómo el polígono de control se reduce hasta el punto de la curva correspondiente al parámetro elegido.

## Fundamentos

Para un parámetro t entre 0 y 1 y puntos A y B, la interpolación (1 − t)A + tB toma un punto del segmento AB. Con n + 1 puntos de control, se empieza en el nivel 0 y se aplican los mismos pesos a cada pareja consecutiva del nivel anterior.

```text
Pᵢ⁽⁰⁾ = Pᵢ
Pᵢ⁽ʳ⁾ = (1 − t) Pᵢ⁽ʳ⁻¹⁾ + t Pᵢ₊₁⁽ʳ⁻¹⁾,   i = 0, …, n − r
B(t) = P₀⁽ⁿ⁾
```

Cada nivel tiene un punto menos. Al desarrollar la recurrencia, los términos que avanzan i veces hacia el siguiente punto y permanecen n − i veces en el índice actual aportan el mismo peso tⁱ(1 − t)ⁿ⁻ⁱ. Hay C(n, i) formas de ordenar esos avances. Se obtiene así el peso de Bernstein de Pᵢ y ambos métodos describen la misma curva en aritmética exacta.

## Cálculo manual de la cúbica

Se usan los mismos puntos del [módulo de Bernstein](Bernstein.md), en orden P₀ = (100; 300), P₁ = (150; 100), P₂ = (350; 100) y P₃ = (400; 300). Las coordenadas de cada punto se separan con punto y coma.

Para t = 0,25 se aplican los pesos 3/4 y 1/4 a ambos ejes en cada nivel.

```text
Nivel 1
Q₀ = (3/4)(100; 300) + (1/4)(150; 100) = (112,5; 250)
Q₁ = (3/4)(150; 100) + (1/4)(350; 100) = (200; 100)
Q₂ = (3/4)(350; 100) + (1/4)(400; 300) = (362,5; 150)

Nivel 2
R₀ = (3/4)(112,5; 250) + (1/4)(200; 100) = (134,375; 212,5)
R₁ = (3/4)(200; 100) + (1/4)(362,5; 150) = (240,625; 112,5)

Nivel 3
B = (3/4)(134,375; 212,5) + (1/4)(240,625; 112,5) = (160,9375; 187,5)
```

En t = 0,5 se promedian las parejas consecutivas. En t = 0,75 se repite el cálculo con los pesos 1/4 y 3/4. En los extremos cada pareja aporta su primer punto para t = 0 y su segundo punto para t = 1.

| t | Nivel 1 (Q₀, Q₁, Q₂) | Nivel 2 (R₀, R₁) | Nivel 3 B(t) |
| --- | --- | --- | --- |
| 0 | (100; 300), (150; 100), (350; 100) | (100; 300), (150; 100) | (100; 300) |
| 0,25 | (112,5; 250), (200; 100), (362,5; 150) | (134,375; 212,5), (240,625; 112,5) | (160,9375; 187,5) |
| 0,5 | (125; 200), (250; 100), (375; 200) | (187,5; 150), (312,5; 150) | (250; 150) |
| 0,75 | (137,5; 150), (300; 100), (387,5; 250) | (259,375; 112,5), (365,625; 212,5) | (339,0625; 187,5) |
| 1 | (150; 100), (350; 100), (400; 300) | (350; 100), (400; 300) | (400; 300) |

## Implementación y visualización

`DeCasteljau.EvaluarPunto` devuelve un punto para cualquier t en [0, 1]. `GenerarCurva` reutiliza un arreglo de coordenadas `double` para evaluar las m muestras en tᵢ = i/(m − 1), incluidos ambos extremos. `ObtenerNiveles` usa la misma interpolación y guarda una copia de cada nivel para dibujarlo. Los tres métodos reciben los puntos de control en el mismo orden y conservan los datos de entrada.

Para reproducir el ejemplo, ingresa los cuatro puntos, selecciona cinco muestras y pulsa **Generar curva**. La línea azul continua corresponde a Bernstein y la naranja discontinua a De Casteljau. Ambas se dibujan como segmentos entre las muestras calculadas. El trazo naranja es más fino para mantener visibles los dos métodos cuando coinciden. Con cinco muestras se ven los cinco vértices unidos por segmentos. Aumenta la cantidad de muestras para ver una aproximación más suave.

Activa **Interpolaciones en t** e ingresa, por ejemplo, 0,25. El polígono de control es gris, los niveles intermedios alternan violeta y verde azulado, y el marcador rojo indica B(t). Este control evalúa el t elegido aunque esté entre dos muestras del trazado.

## Estabilidad, ventajas y complejidad

En aritmética exacta cada interpolación es una combinación convexa. Por ello el resultado queda dentro de la envolvente convexa de los puntos de control. En punto flotante, la fórmula evita calcular coeficientes binomiales grandes y multiplicarlos por potencias pequeñas. Esto reduce el riesgo de desbordamiento o subdesbordamiento que presenta la evaluación directa implementada en el proyecto a grados altos. La precisión relativa aún puede deteriorarse si el problema está mal condicionado, por ejemplo si coordenadas de signos opuestos casi se cancelan. [Hermes, Compensated de Casteljau algorithm in K times the working precision](https://arxiv.org/abs/1808.10387)

Para una muestra interior de grado n se hacen n(n + 1)/2 interpolaciones. El tiempo por muestra es O(n²) y el espacio auxiliar es O(n). Con m muestras, el peor caso es O(mn²) y la salida ocupa O(m). Guardar todos los niveles para visualizar un t requiere O(n²) espacio. Bernstein recorre O(n) pesos por muestra en esta implementación, contando cada llamada a `Math.Pow` como una operación de costo fijo respecto al grado.

La evaluación directa tiene menor costo asintótico al generar muchas muestras. Para grados bajos, el tiempo efectivo depende también del costo de las potencias y debe medirse. De Casteljau ofrece las interpolaciones necesarias para visualizar la construcción y evita los problemas de rango asociados a los pesos calculados por separado.
