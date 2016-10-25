package application;

import java.math.BigDecimal;
import java.math.RoundingMode;
import java.util.function.BiFunction;

public interface Calculator {
	public static final int DIVISION_SCALE = 10;
	
	public Calculator numericButtonPressed(int button);
	public Calculator operatorButtonPressed( CalculatorOperator operator);
	public Calculator dotButtonPressed();
	public Calculator clearButtonPressed();
	public Calculator invertSignButtonPressed();
	public Calculator squareRootButtonPressed();
	public Calculator equalButtonPressed();
	public String getDisplayedValue();
	
	public static enum CalculatorOperator {
		ADD((acc, scr) -> acc.add(scr)), SUBTRACT((acc, scr) -> acc.subtract(scr)), MULTIPLY(
				(acc, scr) -> acc.multiply(scr)), DIVIDE((acc, scr) -> {
					try {
						return acc.divide(scr);
					} catch (Exception e) {
						return acc.divide(scr, DIVISION_SCALE, RoundingMode.HALF_UP);
					}
				});

		private CalculatorOperator(BiFunction<BigDecimal, BigDecimal, BigDecimal> actionFunction) {
			this.actionFunction = actionFunction;
		}

		private BiFunction<BigDecimal, BigDecimal, BigDecimal> actionFunction;

		public BigDecimal executeAction(BigDecimal acc, BigDecimal scr) {
			return actionFunction.apply(acc, scr);
		}
	}

	
}
