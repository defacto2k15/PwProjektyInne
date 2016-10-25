package application;

import java.math.BigDecimal;

public class ErrorCalculatorState implements Calculator {
	private String errorMessage;
	
	public ErrorCalculatorState(String errorMessage) {
		this.errorMessage = errorMessage;
	}

	@Override
	public Calculator numericButtonPressed(int button) {
		return this;
	}

	@Override
	public Calculator operatorButtonPressed(CalculatorOperator operator) {
		return this;
	}

	@Override
	public Calculator dotButtonPressed() {
		return this;
	}

	@Override
	public Calculator clearButtonPressed() {
		return new ScreenViewCalculatorState(BigDecimal.ZERO, BigDecimal.ZERO, CalculatorOperator.ADD);
	}

	@Override
	public Calculator invertSignButtonPressed() {
		return this;
	}

	@Override
	public Calculator squareRootButtonPressed() {
		return this;
	}

	@Override
	public Calculator equalButtonPressed() {
		return this;
	}

	@Override
	public String getDisplayedValue() {
		return errorMessage;
	}

}
