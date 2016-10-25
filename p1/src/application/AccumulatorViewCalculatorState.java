package application;

import java.math.BigDecimal;

public class AccumulatorViewCalculatorState extends BaseCalculatorState {

	protected AccumulatorViewCalculatorState(BigDecimal accumulator, BigDecimal screenValue,
			CalculatorOperator currentCalculatorOperator) {
		super(accumulator, screenValue, currentCalculatorOperator);
	}

	@Override
	public Calculator numericButtonPressed(int button) {
		return new ScreenViewCalculatorState(BigDecimal.ZERO, BigDecimal.ZERO, CalculatorOperator.ADD)
				.numericButtonPressed(button);
	}

	@Override
	public Calculator operatorButtonPressed(CalculatorOperator operator) {
		return new ScreenViewCalculatorState(accumulator, BigDecimal.ZERO, operator);
	}

	@Override
	public Calculator dotButtonPressed() {
		return new ScreenViewCalculatorState(BigDecimal.ZERO, BigDecimal.ZERO, CalculatorOperator.ADD)
				.dotButtonPressed();
	}

	@Override
	public Calculator clearButtonPressed() {
		return new ScreenViewCalculatorState(BigDecimal.ZERO, BigDecimal.ZERO, CalculatorOperator.ADD);
	}

	@Override
	public Calculator invertSignButtonPressed() {
		return new AccumulatorViewCalculatorState(accumulator.negate(), screenValue, currentCalculatorOperator);
	}

	@Override
	public Calculator squareRootButtonPressed() {
		return new AccumulatorViewCalculatorState(new BigDecimal(Math.sqrt(accumulator.doubleValue())), screenValue,
				currentCalculatorOperator);
	}

	@Override
	public Calculator equalButtonPressed() {
		return new ScreenViewCalculatorState(accumulator,
				screenValue, currentCalculatorOperator).equalButtonPressed();
	}

	@Override
	public String getDisplayedValue() {
		return bigDecimalToShortString(accumulator);
	}

}
