package application;

import java.math.BigDecimal;
import java.math.RoundingMode;
import java.util.function.*;

public class RootCalculator implements Calculator{
	Calculator currentCalculatorState;
	
	public RootCalculator(){
		currentCalculatorState = new ScreenViewCalculatorState(BigDecimal.ZERO, BigDecimal.ZERO, CalculatorOperator.ADD);
	}
	
	@Override
	public Calculator numericButtonPressed(int button) {
		currentCalculatorState = currentCalculatorState.numericButtonPressed(button);
		return currentCalculatorState;
	}

	@Override
	public Calculator operatorButtonPressed(CalculatorOperator operator) {
		currentCalculatorState = currentCalculatorState.operatorButtonPressed(operator);
		return currentCalculatorState;
	}

	@Override
	public Calculator dotButtonPressed() {
		currentCalculatorState = currentCalculatorState.dotButtonPressed();
		return currentCalculatorState;
	}

	@Override
	public Calculator clearButtonPressed() {
		currentCalculatorState = currentCalculatorState.clearButtonPressed();
		return currentCalculatorState;
	}

	@Override
	public Calculator invertSignButtonPressed() {
		currentCalculatorState = currentCalculatorState.invertSignButtonPressed();
		return currentCalculatorState;
	}

	@Override
	public Calculator squareRootButtonPressed() {
		currentCalculatorState = currentCalculatorState.squareRootButtonPressed();
		return currentCalculatorState;
	}

	@Override
	public Calculator equalButtonPressed() {
		currentCalculatorState = currentCalculatorState.equalButtonPressed();
		return currentCalculatorState;
	}

	@Override
	public String getDisplayedValue() {
		return currentCalculatorState.getDisplayedValue();
	}
}
